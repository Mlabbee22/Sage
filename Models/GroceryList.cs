using Microsoft.EntityFrameworkCore;
using SAGE.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SAGE.Models
{

    [Table("groceryList")]
    public partial class GroceryList
    {
        [Key]
        [Column("userId")]
        public string UserId { get; set; }
        [Key]
        [Column("ingredientId")]
        public int IngredientId { get; set; }
        [Column("quantity")]
        public string Quantity { get; set; }
        [Column("notes")]
        [StringLength(255)]
        [Unicode(false)]
        public string Notes { get; set; }

        [ForeignKey(nameof(IngredientId))]
        [InverseProperty("GroceryLists")]
        public virtual Ingredient Ingredient { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SAGEUser.GroceryLists))]
        public virtual SAGEUser User { get; set; }
    }
}
