using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
    public class Review : BaseEntity<Guid>
    {
        [Column("rating")]
        [Range(0, 5)]
        public int Rating { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public Guid? UserId { get; set; }

        public User? User { get; set; }

        public Guid? ImageId { get; set; }

        public SharedImage? Image { get; set; }

    }
}
