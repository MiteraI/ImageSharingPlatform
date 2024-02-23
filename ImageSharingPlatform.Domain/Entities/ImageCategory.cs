using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
    public class ImageCategory : BaseEntity<Guid>
    {
        [Column("category_name")]
        public string CategoryName { get; set; }

        [Column("description")]
        public string? Description { get; set; }

    }
}
