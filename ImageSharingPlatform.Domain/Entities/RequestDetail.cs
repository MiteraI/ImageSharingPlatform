using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
	public class RequestDetail : BaseEntity<Guid>
	{
		public Guid RequestId { get; set; }
		public ImageRequest Request { get; set; }
		public string? Title { get; set; }
		public string? Comment { get; set; }
		public DateTime CreatedAt { get; set; }

        [Column("expected_time")]
        public DateTime ExpectedTime { get; set; }

        public double? NewPrice { get; set; }
		public Guid? UserId { get; set; }
		public User? User { get; set; }


	}
}
