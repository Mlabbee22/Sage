using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace SAGE.Models
{
    [Table("ingredient")]
    public partial class Ingredient
    {
        public Ingredient()
        {
            GroceryLists = new HashSet<GroceryList>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("department")]
        [Unicode(false)]
        public string Department { get; set; }
        [Column("allergens")]
        [Unicode(false)]
        public string Allergens { get; set; }
        [Column("name")]
        [StringLength(255)]
        [Unicode(false)]
        public string Name { get; set; }

        [InverseProperty(nameof(GroceryList.Ingredient))]
        public virtual ICollection<GroceryList> GroceryLists { get; set; }

    }
}
