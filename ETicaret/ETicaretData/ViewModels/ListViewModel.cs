using System;
using ETicaretData.Entities;

namespace ETicaretData.ViewModels
{
	public class ListViewModel
	{
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public ListViewModel()
        {
            Products = new List<Product>();
        }
    }
}

