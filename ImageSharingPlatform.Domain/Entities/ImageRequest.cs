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
        public double Price { get; set; }

        [Column("status")]
        public RequestStatus RequestStatus { get; set; }

        [Column("created_at")]
        public DateTime CreateTime { get; set; }

        [Column("image")]
        public byte[]? ImageBlob { get; set; }

        public Guid? RequesterUserId { get; set; }
        public User? RequesterUser { get; set; }

        public Guid? ArtistId { get; set; }
        public User? Artist { get; set; }
    }
}
