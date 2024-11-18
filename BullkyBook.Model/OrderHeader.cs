using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.Model
{
	public class OrderHeader
	{
		[Key]
		public int Order_id { get; set; }

		public string? AppUserId { get; set; }
		[ForeignKey("AppUserId")]
		[ValidateNever]

		public AppUser? AppUser { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
		public DateTime ShippingDate { get; set; }

        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
		public string? PaymentStatus { get; set; }
		public string? TrackingNumber { get; set; }
		public string? Carrier{ get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        public string? User_name { get; set; }
        public string? StAddress { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postalCode { get; set; }
        public string? PhoneNumber { get; set; }





    }
}
