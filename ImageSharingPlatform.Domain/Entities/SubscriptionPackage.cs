using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
	public class SubscriptionPackage : BaseEntity<Guid>
	{
		public double Price { get; set; }
		public string? Description { get; set; }
		public Guid? ArtistId { get; set; }
		public User? Artist { get; set; }
	}
}
