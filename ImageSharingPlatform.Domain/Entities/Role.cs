using ImageSharingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
    public class Role : BaseEntity<Guid>
    {
        [Column("role")]
        public UserRole UserRole { get; set; }
    }
}
