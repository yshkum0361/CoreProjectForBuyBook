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
	public class OrderDetails
	{
		[Key]
		public int Id { get; set; }

		[Required]
        public int Order_Id { get; set; }
		[ForeignKey("Order_Id")]
		[ValidateNever]

		public OrderHeader OrderHeader { get; set; }
		[Required]

		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
        [ValidateNever]
        public ProductModel ProductModel { get; set; }

		public int Count { get; set; }
		public double price { get; set; }

    }
}
