﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.Model.ViewModels
{
	public class ShoppingCartVm
	{
		public IEnumerable<ShoppingCart>? ListCart { get; set; }

		//public double CartTotal { get; set; }
		public OrderHeader OrderHeader { get; set; }
	}
}
