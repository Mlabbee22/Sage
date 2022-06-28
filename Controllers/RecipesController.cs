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
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index(string searchString, string user = "Your Own", int search = 0)
        {
            if (user == null) { return View("Error"); }
            var myname = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (myname == null) { return View("Index"); }
            bool checkIfSubscribed = myname.PremiumMember;

            if (checkIfSubscribed) { ViewData["Subscribed?"] = "true"; }
            else { ViewData["Subscribed?"] = "false"; }

            myname = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();

            var mySavedRecipes = _context.SavedRecipes.Where(x => x.UserId == myname.Id).ToList();
            if (mySavedRecipes != null)
            {
                List<Recipe> myRecipes = new List<Recipe>();
                foreach (var recipe in mySavedRecipes)
                {
                    var thisRecipe = _context.Recipe.Where(x => x.Id == recipe.RecipeId).FirstOrDefault();
                    thisRecipe.AuthorNavigation = _context.Account.Where(x => x.Id == thisRecipe.Author).FirstOrDefault();
                    myRecipes.Add(thisRecipe);
                }
                ViewData["MyRecipes"] = myRecipes;
                ViewData["HasRecipes"] = "yes";
            }
            else { ViewData["HasRecipes"] = "no"; }

            if (String.IsNullOrEmpty(searchString))
            {
                
                var applicationDbContext = _context.Recipe.Include(r => r.AuthorNavigation).Where(x => x.AuthorNavigation == myname);
                return View(await applicationDbContext.ToListAsync());
            }

            else if (!String.IsNullOrEmpty(searchString))
            {
                if (search == 1)
                {
                    var recipes = _context.Recipe.Include(r => r.AuthorNavigation).Where(x => x.Name.Contains(searchString) && x.AuthorNavigation == myname);
                    return View(await recipes.ToListAsync());
                }
                else if (search == 2)
                {
                    var recipes = _context.Recipe.Include(r => r.AuthorNavigation).Where(x => x.Tags.Contains(searchString) && x.AuthorNavigation == myname);
                    return View(await recipes.ToListAsync());
                }
                else
                { return View(); }

            }

            else 
            { return View(); }

        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.AuthorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            var myname = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault().Id;
            var isFave = _context.SavedRecipes.Where(x => x.RecipeId == id && x.UserId == myname).FirstOrDefault();
            if (isFave == null)
            {
                ViewData["RecipeSaved"] = "no";
                ViewData["isFave"] = "no";
            }
            else
            {
                ViewData["RecipeSaved"] = "yes";
                if (isFave.isFavorite) { ViewData["isFave"] = "yes"; }
                else { ViewData["isFave"] = "no"; }
            }

            

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["Author"] = new SelectList(_context.Account.Where(c => c.UserName == User.Identity.Name));

            //var author = _context.Account.Where(c => c == User.Claims);
            //ViewData["Author"] = whoever the logged-in user is
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Steps,Tags,Ingredients,Rating,Name,Author,isPublic")] Recipe recipe)
        {
            recipe.Rating = -1;
            recipe.AuthorNavigation = _context.Account.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Author"] = new SelectList(_context.Account, "Id", "Id", recipe.Author);
            
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            
            if (recipe == null)
            {
                return NotFound();
            }
            var author = _context.Account.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            if (recipe.AuthorNavigation != author)
            {
                if (!author.PremiumMember)
                {
                    return RedirectToAction("Subscribe", "Home");
                }
                else
                {
                    var recipeCopy = _context.Recipe.FirstOrDefault();
                    var copyToAdd = new Recipe();


                    copyToAdd.Author = author.Id;
                    copyToAdd.Description = recipe.Description;
                    copyToAdd.Reviews = recipe.Reviews;
                    copyToAdd.SavedRecipes = recipe.SavedRecipes;
                    copyToAdd.AuthorNavigation = author;
                    copyToAdd.Steps = recipe.Steps;
                    copyToAdd.Rating = -1;
                    copyToAdd.Name = recipe.Name;
                    copyToAdd.isPublic = false;
                    copyToAdd.Tags = recipe.Tags;
                    copyToAdd.Ingredients = recipe.Ingredients;
                    _context.Recipe.Add(copyToAdd);
                    _context.SaveChanges();
                    var urlId = _context.Recipe.Where(x => x.AuthorNavigation == author && x.Name == copyToAdd.Name).FirstOrDefault().Id;
                    string redirectUrl = "https://localhost:7189/Recipes/edit?id=" + urlId;
                    return RedirectToAction("Edit", new { id = urlId });
                    //return Redirect(redirectUrl);
                }
            }
            
            return View(recipe);
        }


        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Steps,Tags,Ingredients,Rating,Name,Author,isPublic")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var myRecipe = _context.Recipe.Where(x => x.Id == recipe.Id).FirstOrDefault();
                    myRecipe.Author = _context.Account.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                    myRecipe.Name = recipe.Name;
                    myRecipe.Description = recipe.Description;
                    myRecipe.Ingredients = recipe.Ingredients;
                    myRecipe.Tags = recipe.Tags; 
                    myRecipe.Rating = recipe.Rating;
                    myRecipe.AuthorNavigation = _context.Account.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                    myRecipe.Steps = recipe.Steps;
                    myRecipe.isPublic = recipe.isPublic;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            ViewData["Author"] = new SelectList(_context.Account, "Id", "Id", recipe.Author);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.AuthorNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipe.FindAsync(id);
            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Explore(string searchString, string user = "Your Own", int search = 0)
        {

            if (user == null) { return View("Error"); }
            var myname = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (myname == null) { return View("Index"); }
            bool checkIfSubscribed = myname.PremiumMember;


            if (checkIfSubscribed) { ViewData["Subscribed?"] = "true"; }
            else { ViewData["Subscribed?"] = "false"; }

            if (String.IsNullOrEmpty(searchString))
            {
                var applicationDbContext = _context.Recipe.Include(r => r.AuthorNavigation).Where(c=>c.isPublic);
                return View(await applicationDbContext.ToListAsync());
            }

            else if (!String.IsNullOrEmpty(searchString))
            {
                if (search == 1)
                {
                    var recipes = _context.Recipe.Include(r => r.AuthorNavigation).Where(x => x.Name.Contains(searchString) && x.isPublic);
                    return View(await recipes.ToListAsync());
                }
                else if (search == 2)
                {
                    var recipes = _context.Recipe.Include(r => r.AuthorNavigation).Where(x => x.Tags.Contains(searchString) && x.isPublic);
                    return View(await recipes.ToListAsync());
                }
                else
                { return View(); }

                
            }
            else
            { return View(); }

        }

        public async Task<IActionResult> SaveRecipe(int id)
        {
            var myname = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            var recipeToChange = _context.SavedRecipes.Where(x => x.RecipeId == id && x.User == myname).FirstOrDefault();
            var myRecipe = _context.Recipe.Where(x => x.Id == id).FirstOrDefault();
            if (recipeToChange == null)
            {
                var recipeToAdd = new SavedRecipe
                {
                    UserId = myname.Id,
                    User = myname,
                    RecipeId = id,
                    Recipe = myRecipe
                };
                _context.Add(recipeToAdd);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = id });

        }

        public async Task<IActionResult> unSaveRecipe(int id)
        {
            var myname = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            var recipeToChange = _context.SavedRecipes.Where(x => x.RecipeId == id && x.UserId == myname.Id).FirstOrDefault();
            if (recipeToChange == null)
            {
                //you shouldn't get here if the user has saved recipe
                return RedirectToAction("Details", new { id = id });

            }
            _context.Remove(recipeToChange);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = id });

        }

        public async Task<IActionResult> FavoriteRecipe(int id)
        {
            var myname = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault().Id;
            var recipeToChange = _context.SavedRecipes.Where(x => x.RecipeId == id && x.UserId == myname).FirstOrDefault();
            if (recipeToChange != null)
            {
                recipeToChange.isFavorite = !recipeToChange.isFavorite;
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = id });
            
        }
    }
}
