using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [StringLength(50)]
        [MaxLength(50)]
        [Column("first_name")]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [MaxLength(50)]
        [Column("last_name")]
        public string? LastName { get; set; }

        [Url]
        [StringLength(256)]
        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [EmailAddress]
        [MinLength(5)]
        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; }

        public ICollection<Role>? Roles { get; set; }
    }
}
