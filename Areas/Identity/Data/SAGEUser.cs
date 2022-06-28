using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAGE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace SAGE.AspNetCore.Identity.Data
{

    public class SAGEUser : IdentityUser
    {
        [PersonalData]
        public SAGEUser()
        {
            GroceryLists = new HashSet<GroceryList>();
            Recipes = new HashSet<Recipe>();
            Reviews = new HashSet<Review>();
            Followings = new HashSet<SAGEUser>();
            Users = new HashSet<SAGEUser>();
            SavedRecipes = new HashSet<SavedRecipe>();
            PremiumMember = false;
        }

        [PersonalData]
        [StringLength(255)]
        [Unicode(false)]
        public string FirstName { get; set; }

        [PersonalData]
        [StringLength(255)]
        [Unicode(false)]
        public string LastName { get; set; }

        [PersonalData]
        [DefaultValue(false)]
        public bool PremiumMember { get; set; } = false;

        [PersonalData]
        [Column("allergens")]
        [Unicode(false)]
        public string Allergens { get; set; }

        




        [InverseProperty(nameof(GroceryList.User))]
        public virtual ICollection<GroceryList> GroceryLists { get; set; }
        [InverseProperty(nameof(Recipe.AuthorNavigation))]
        public virtual ICollection<Recipe> Recipes { get; set; }
        [InverseProperty(nameof(Review.User))]
        public virtual ICollection<Review> Reviews { get; set; }
        [InverseProperty(nameof(SavedRecipe.User))]
        public virtual ICollection<SavedRecipe> SavedRecipes { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty(nameof(SAGEUser.Users))]
        public virtual ICollection<SAGEUser> Followings { get; set; }
        [ForeignKey("FollowingId")]
        [InverseProperty(nameof(SAGEUser.Followings))]
        public virtual ICollection<SAGEUser> Users { get; set; }
    }
}
