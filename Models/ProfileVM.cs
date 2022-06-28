using SAGE.AspNetCore.Identity.Data;

namespace SAGE.Models
{
    public class ProfileVM
    {
        public string DisplayName { get; set; }
        public List<Recipe>? FavoriteRecipes { get; set; } = new List<Recipe>();
        public List<Recipe>? PublicRecipes { get; set; } = new List<Recipe>();
        public List<SAGEUser>? Followers { get; set; } = new List<SAGEUser>();


        public string Email { get; set; }
        public bool AreYouFollowing { get; set; }

        public bool AreYouSubscribed { get; set; }

    }
}
