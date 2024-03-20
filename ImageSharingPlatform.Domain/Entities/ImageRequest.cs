using ImageSharingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
    public class ImageRequest : BaseEntity<Guid>
    {
        [Column("title")]
        [StringLength(50)]
        public string Title { get; set; }

        [Column("description")]
        [StringLength(256)]
        public string? Description { get; set; }

        [Column("price")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(100000.00, 1000000.00, ErrorMessage = "Price must be between 100,000 and 1,000,000")]
        public double Price { get; set; }

        [Column("status")]
        public RequestStatus RequestStatus { get; set; }

        [Column("created_at")]
        public DateTime CreateTime { get; set; }

        [Column("expected_time")]
        public DateTime ExpectedTime { get; set; }  

        [Column("image")]
        public byte[]? ImageBlob { get; set; }

        public Guid? RequesterUserId { get; set; }
        public User? RequesterUser { get; set; }

        public Guid ArtistId { get; set; }
        public User Artist { get; set; }
	}
}
