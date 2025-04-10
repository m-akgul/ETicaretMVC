using System;
using ETicaretData.Entities;

namespace ETicaretData.ViewModels
{
	public class CartItem
	{
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem()
		{
		}
	}
}

