using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAGE.Data;
using SAGE.Models;

namespace SAGE.Controllers
{
    public class GroceryListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroceryListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GroceryLists
        public async Task<IActionResult> Index()
        {
            var user = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();

            if (!user.PremiumMember)
            {
                return RedirectToAction("Subscribe", "Home");
            }

            var myIdentity = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            ViewData["GroceryList"] = _context.GroceryList.Where(gl => gl.User == myIdentity).ToList();

            var myList = _context.GroceryList.Include(g => g.Ingredient).Include(g => g.User).Where(x => x.UserId == myIdentity.Id);


            return View(await myList.ToListAsync());
        }

        // GET: GroceryLists/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groceryList = await _context.GroceryList
                .Include(g => g.Ingredient)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (groceryList == null)
            {
                return NotFound();
            }

            return View(groceryList);
        }

        // GET: GroceryLists/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name");
            ViewData["IngredientName"] = new SelectList(_context.Ingredient, "Id", "Name");

            return View();
        }

        // POST: GroceryLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,IngredientId,Quantity,Notes")] GroceryList groceryList)
        {
            groceryList.User = _context.Account.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            groceryList.UserId = _context.Account.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
            var thisUserId = _context.Account.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
            var myListValue = _context.GroceryList.Where(gl => gl.UserId == thisUserId && gl.IngredientId == groceryList.IngredientId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (myListValue != null)
                {
                    return RedirectToAction("Edit", new { id = thisUserId, ingredientId = groceryList.IngredientId });
                }
                _context.Add(groceryList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientName"] = new SelectList(_context.Ingredient, "Id", "Name");



            ViewData["UserId"] = new SelectList(_context.Account, "Id", "Id", groceryList.UserId);
            return View(groceryList);
        }

        // GET: GroceryLists/Edit/5
        public async Task<IActionResult> Edit(string id, int ingredientId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groceryList = await _context.GroceryList.FindAsync(id, ingredientId);
            if (groceryList == null)
            {
                return NotFound();
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name", groceryList.IngredientId);
            ViewData["UserId"] = new SelectList(_context.Account, "Id", "Id", groceryList.UserId);
            ViewData["IngredientName"] = _context.Ingredient.Where(x => x.Id == ingredientId).FirstOrDefault().Name;
            return View(groceryList);
        }

        // POST: GroceryLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,IngredientId,Quantity,Notes")] GroceryList groceryList)
        {
            groceryList.Ingredient = _context.Ingredient.Where(x => x.Id == groceryList.IngredientId).FirstOrDefault();
            groceryList.User = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            groceryList.UserId = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault().Id;
            if (id != groceryList.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groceryList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroceryListExists(groceryList.UserId, groceryList.IngredientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Id", groceryList.IngredientId);
            ViewData["UserId"] = new SelectList(_context.Account, "Id", "Id", groceryList.UserId);
            return View(groceryList);
        }

        // GET: GroceryLists/Delete/5
        public async Task<IActionResult> Delete(string id, int ingredientId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groceryList = await _context.GroceryList.FindAsync(id, ingredientId);
            groceryList.User = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            groceryList.Ingredient = _context.Ingredient.Where(i => i.Id == ingredientId).FirstOrDefault();
            groceryList.IngredientId = _context.Ingredient.Where(i => i.Id == ingredientId).FirstOrDefault().Id;

            if (groceryList == null)
            {
                return NotFound();
            }

            return View(groceryList);
        }

        // POST: GroceryLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, int ingredientId)
        {
            var groceryList = await _context.GroceryList.FindAsync(id, ingredientId);
            _context.GroceryList.Remove(groceryList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroceryListExists(string id, int ingId)
        {
            return _context.GroceryList.Any(e => e.UserId == id && e.IngredientId == ingId);
        }
    }
}
