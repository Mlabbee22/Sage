using SAGE.AspNetCore.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAGE.Models
{
    [Table("SavedRecipe")]
    public class SavedRecipe
    {
        [Key]
        [Column("userId")]
        public string UserId { get; set; }


        [Key]
        [Column("recipeId")]
        public int RecipeId { get; set; }

        [Column("isFavorite")]
        [Required]
        public bool isFavorite { get; set; } = false;


        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("SavedRecipes")]
        public virtual Recipe Recipe { get; set; }



        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SAGEUser.SavedRecipes))]
        public virtual SAGEUser User { get; set; }
    }
}
