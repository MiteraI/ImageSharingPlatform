using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
	public class OwnedSubscription : BaseEntity<Guid>
	{
		public DateTime PurchasedTime { get; set; }
		public Guid? SubscriptionPackageId { get; set; }
		public SubscriptionPackage? SubscriptionPackage { get; set; }
		public Guid? UserId { get; set; }
		public User? User { get; set; }
	}
}
