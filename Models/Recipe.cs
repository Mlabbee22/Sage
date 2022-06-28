using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SAGE.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SAGE.Models
{
    [Table("Recipe")]
    public partial class Recipe
    {
        public Recipe()
        {
            Reviews = new HashSet<Review>();
            SavedRecipes = new HashSet<SavedRecipe>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("description")]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("steps")]
        [Unicode(false)]
        public string Steps { get; set; }
        [Column("tags")]
        [Unicode(false)]
        public string Tags { get; set; }

        [Column("isPublic")]
        [Unicode(false)]
        public Boolean isPublic { get; set; }

        [Column("Ingredients")]
        [Unicode(false)]
        public string Ingredients { get; set; }


        [Column("rating", TypeName = "decimal(18, 2)")]
        public decimal? Rating { get; set; }
        [Column("name")]
        [StringLength(255)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column("author")]
   
        public string Author { get; set; }

        [ForeignKey(nameof(Author))]
        [InverseProperty(nameof(SAGEUser.Recipes))]
        public virtual SAGEUser AuthorNavigation { get; set; }


        [InverseProperty(nameof(Review.Recipe))]
        public virtual ICollection<Review> Reviews { get; set; }



        [InverseProperty(nameof(SavedRecipe.Recipe))]
        public virtual ICollection<SavedRecipe> SavedRecipes { get; set; }

        public static explicit operator Recipe(EntityEntry<Recipe> v)
        {
            throw new NotImplementedException();
        }
    }
}
