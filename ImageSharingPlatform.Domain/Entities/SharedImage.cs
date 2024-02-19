using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
    public class SharedImage : BaseEntity<Guid>
    {
        public string ImageName;
            
        public string? ImageUrl;

        public Guid? ImageCategoryId { get; set; }

        public ImageCategory? ImageCategory { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set;}
        public bool IsPremium { get; set; }

        public Guid? ArtistId { get; set; }

        public User? Artist { get; set; }

        public ICollection<Review>? Reviews { get; set; }
    }
}
