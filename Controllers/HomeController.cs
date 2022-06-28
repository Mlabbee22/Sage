using Microsoft.AspNetCore.Mvc;
using SAGE.Models;
using System.Diagnostics;
using SAGE.Data;
using SAGE.AspNetCore.Identity.Data;

namespace SAGE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {

            _context = context;//hsdfffffff

            _logger = logger;//agslhfjshgfkusg
        }

        public IActionResult Index(string user = "Your Own")
        {
            if (user == null) { return View("Error"); }
            var myname = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (myname == null) { return View("Index"); }
            bool checkIfSubscribed = myname.PremiumMember;
           

            if (checkIfSubscribed) { ViewData["Subscribed?"] = "true"; }
            else { ViewData["Subscribed?"] = "false"; }

            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Subscribe(string user = "Your Own")
        {
            if (user == null) { return View("Error"); }
            var myname = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (myname == null) { return View("Index"); }
            bool checkIfSubscribed = myname.PremiumMember;

            //Not currently using the ViewData right now. 
            //probably will be needed for changing the button from "follow" to "unfollow"
            if (checkIfSubscribed) { ViewData["Subscribed?"] = "true"; }
            else { ViewData["Subscribed?"] = "false"; }
            ViewData["FollowId"] = user;

            return View();
        }

        public IActionResult CustomerSupport(string user = "Your Own")
        {
        
                    if (user == null) { return View("Error"); }
                    var myname = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
                    if (myname == null) { return View("Index"); }
                    bool checkIfSubscribed = myname.PremiumMember;


                    if (checkIfSubscribed) { ViewData["Subscribed?"] = "true"; }
                    else { ViewData["Subscribed?"] = "false"; }

                    return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ViewProfile(string user = "Your Own")
        {

            if (user == null) { return View("Error"); }
            //if (user == "Your Own") { return View("Index"); }
            var myname = _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            var profile = _context.Users.Where(x => x.Id == user).DefaultIfEmpty().FirstOrDefault();
            if (profile == null) { profile = myname; }
            if (profile == null || myname == null) { return View("Index"); }
            bool checkIfFollowing = _context.Following.Where(c => c.UserId==myname.Id && c.FollowingId == profile.Id).Any();
            ProfileVM profileVM = new ProfileVM
            {
                DisplayName = profile.FirstName,
                AreYouFollowing = checkIfFollowing,
                Email = user,
            };

            if (user == myname.Id)
            {
                foreach (var publicR in _context.Recipe.Where(r => r.Author == myname.Id && r.isPublic).ToList())
                {
                    profileVM.PublicRecipes.Add(publicR);
                }
                foreach (var faveR in _context.SavedRecipes.Where(r => r.User == myname && r.isFavorite).ToList())
                {
                    var rectoadd = _context.Recipe.Where(r => r.Id == faveR.RecipeId).FirstOrDefault();
                    profileVM.FavoriteRecipes.Add(rectoadd);
                }
                foreach (var follower in _context.Following.Where(f => f.FollowingId == myname.Id).ToList())
                {
                    var aFollower = _context.Account.Where(a => a.Id == follower.UserId).FirstOrDefault();
                    profileVM.Followers.Add(aFollower);
                }
            }

            else
            {
                foreach (var publicR in _context.Recipe.Where(r => r.Author == profile.Id && r.isPublic).ToList())
                {
                    profileVM.PublicRecipes.Add(publicR);
                }
                foreach (var faveR in _context.SavedRecipes.Where(r => r.User == profile && r.isFavorite).ToList())
                {
                    var rectoadd = _context.Recipe.Where(r => r.Id == faveR.RecipeId).FirstOrDefault();
                    profileVM.FavoriteRecipes.Add(rectoadd);
                }
                foreach (var follower in _context.Following.Where(f => f.FollowingId == profile.Id).ToList())
                {
                    var aFollower = _context.Account.Where(a => a.Id == follower.UserId).FirstOrDefault();
                    profileVM.Followers.Add(aFollower);
                }
            }

            //Not currently using the ViewData right now. 
            //probably will be needed for changing the button from "follow" to "unfollow"
            if (checkIfFollowing) { ViewData["Following?"] = "true"; }
            else { ViewData["Following?"] = "false"; }
            ViewData["FollowId"] = user;

            return View(profileVM);
        }

        //follow user
        //should be called from ViewProfile 
        //should return to ViewProfile
        public async Task<IActionResult> FollowUser(string user)
        {
            if (user == string.Empty) { return View("Error"); }

            var myname =  _context.Users.Where(c => c.UserName == User.Identity.Name).FirstOrDefault();
            var followThisUser = _context.Users.Where(c => c.Id == user).FirstOrDefault();

            if (followThisUser == null || myname == null) { return View("Error"); }

            var follow = _context.Following.Where(x => x.FollowingId== followThisUser.Id && x.UserId == myname.Id).FirstOrDefault();

            if (follow == null)
            {
                var newFollow = new Following
                {
                    UserId = myname.Id,
                    FollowingId = followThisUser.Id,
                    FollowingNavigation = followThisUser
                };
                newFollow.User = myname;

                _context.Following.Add(newFollow);
                _context.SaveChanges();
            }
            else
            {
                _context.Following.Remove(follow);
                _context.SaveChanges();
            }

            string returntoProfileEmail = followThisUser.Id;
            return RedirectToAction("ViewProfile",new {user = returntoProfileEmail});
        }

        //SetPremiumUser
        //should be called from Subscribe
        //should return to Subscribe
        public async Task<IActionResult> SetPremiumUser()
        {
            var currUser = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (currUser == null) { return View("Error"); }
            currUser.PremiumMember = true;
            _context.SaveChanges();

            return RedirectToAction("Subscribe");
        }

        //SetNonPremiumUser
        //should be called from Subscribe
        //should return to Subscribe
        public async Task<IActionResult> SetNonPremiumUser()
        {
            var currUser = _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (currUser == null) { return View("Error"); }
            currUser.PremiumMember = false;
            _context.SaveChanges();

            return RedirectToAction("Subscribe");
        }

    }
}