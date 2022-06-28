using SAGE.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SAGE.Models
{
    
    public partial class Following
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FollowingId { get; set; }

        public virtual SAGEUser FollowingNavigation { get; set; }
        public virtual SAGEUser User { get; set; }
    }
}
