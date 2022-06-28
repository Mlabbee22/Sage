using Microsoft.EntityFrameworkCore;
using SAGE.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SAGE.Models
{
    [Table("review")]
    public partial class Review
    {
        [Key]
        [Column("userID")]
        public string UserId { get; set; }
        [Key]
        [Column("recipeID")]
        public int RecipeId { get; set; }
        [Column("rating")]
        [Range(1, 10, ErrorMessage = "Select a number between {1} and {2}")]
        public int Rating { get; set; }
        [Column("review")]
        [Unicode(false)]
        public string Review1 { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("Reviews")]
        public virtual Recipe Recipe { get; set; }



        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(SAGEUser.Reviews))]
        public virtual SAGEUser User { get; set; }
    }
}
