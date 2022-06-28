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
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(int id)
        {

            if (id == 0)
            {
                var applicationDbContext = _context.Review.Include(r => r.Recipe).Include(r => r.User);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Review.Where(r => r.Recipe.Id == id).Include(r => r.Recipe).Include(r => r.User);
                ViewData["id"] = id;
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Recipe)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create(int recipeID)
        {
            var model = new Review { RecipeId = recipeID };
            
            model.UserId = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault().Id;
            model.RecipeId = _context.Recipe.Where(c => c.Id == recipeID).FirstOrDefault().Id;
            ViewData["RecipeId"] = recipeID;
            ViewData["UserName"] = _context.Account.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().UserName;
            if (_context.Review.Where(r => r.RecipeId == recipeID).Where(r => r.UserId == model.UserId).Count() > 0)
            {
                return RedirectToAction("Edit", new { id = model.UserId,  model.RecipeId });
            }

            return View(model);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RecipeId,Rating,Review1")] Review review)
        {
            review.Recipe = _context.Recipe.Where(c => c.Id == review.RecipeId).FirstOrDefault();
            review.User = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();

                var currReview = _context.Review.Where(r => r.RecipeId == review.Recipe.Id);
                var recipe = review.Recipe;
                int lines = 0;
                decimal total = 0;
                foreach (var line in currReview)
                {
                    total += line.Rating;
                    lines++;
                    _context.Entry<Review>(line).State = EntityState.Detached;
                }


                if (lines == 0)
                {
                    recipe.Rating = -1;
                }
                else
                {
                    recipe.Rating = (total / lines);
                }

                _context.SaveChanges();

                return RedirectToAction("Index", new { id = recipe.Id });
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Id", review.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Account, "Id", "Id", review.UserId);

            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(string id, int recipeID)
        {
            ViewData["id"] = recipeID;
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id, recipeID); 
            if (review == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Id", review.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Account, "Id", "Id", review.UserId);
            return View(review);
        }
        
        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,RecipeId,Rating,Review1")] Review review)
        {
            review.Recipe = _context.Recipe.Where(c => c.Id == review.RecipeId).FirstOrDefault();
            review.User = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();

            if (id != review.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var recipeReviews = _context.Review.Where(r => r.RecipeId == review.RecipeId);
                int lines = 0;
                decimal total = 0;
                foreach (var line in recipeReviews)
                {
                    total += line.Rating;
                    lines++;
                    _context.Entry<Review>(line).State = EntityState.Detached;
                }


                if (lines == 0)
                {
                    review.Recipe.Rating = -1;
                }
                else
                {
                    review.Recipe.Rating = (total / lines);
                }

                _context.SaveChanges();

                return RedirectToAction("Details", "Recipes", new { id = review.RecipeId });
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Id", review.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Account, "Id", "Id", review.UserId);


            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(string id, int recipeID)
        {
            ViewData["id"] = recipeID;
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id, recipeID);
            review.Recipe = _context.Recipe.Where(c => c.Id == review.RecipeId).FirstOrDefault();
            review.User = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            review.RecipeId = _context.Recipe.Where(c => c.Id == review.RecipeId).FirstOrDefault().Id;
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, int recipeID)
        {

            var review = await _context.Review.FindAsync(id, recipeID);
            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            var recipe = _context.Recipe.Where(r => r.Id == recipeID).FirstOrDefault();
            var recipeReviews = _context.Review.Where(r => r.RecipeId == recipeID);
            int lines = 0;
            decimal total = 0;
            foreach (var line in recipeReviews)
            {
                total += line.Rating;
                lines++;
                _context.Entry<Review>(line).State = EntityState.Detached;
            }


            if (lines == 0)
            {
                recipe.Rating = -1;
            }
            else
            {
                recipe.Rating = (total / lines);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", new { id = recipe.Id });
        }

        private bool ReviewExists(string id)
        {
            return _context.Review.Any(e => e.UserId == id);
        }
    }
}
