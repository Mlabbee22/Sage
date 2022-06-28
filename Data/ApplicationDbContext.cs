using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAGE.Models;
using SAGE.AspNetCore.Identity.Data;

namespace SAGE.Data
{
    public class ApplicationDbContext : IdentityDbContext<SAGEUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroceryList>().HasKey(vf=> new {vf.UserId, vf.IngredientId});
            modelBuilder.Entity<Following>().HasKey(vf=> new {vf.UserId, vf.FollowingId});
            modelBuilder.Entity<Review>().HasKey(vf=> new {vf.UserId, vf.RecipeId});
            modelBuilder.Entity<SavedRecipe>().HasKey(sr => new {sr.UserId, sr.RecipeId});
        }
        
        public DbSet<SAGE.Models.Recipe> Recipe { get; set; }
        
        public DbSet<SAGE.Models.Following> Following { get; set; }
        
        public DbSet<SAGE.Models.GroceryList> GroceryList { get; set; }
        
        public DbSet<SAGE.Models.Ingredient> Ingredient { get; set; }
        
        public DbSet<SAGE.Models.Review> Review { get; set; }

        public DbSet<SAGE.Models.SavedRecipe> SavedRecipes { get; set; }
       
        public DbSet<SAGE.AspNetCore.Identity.Data.SAGEUser> Account { get; set; }

    }
}