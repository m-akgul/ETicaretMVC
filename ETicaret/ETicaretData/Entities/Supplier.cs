using System;
namespace ETicaretData.Entities
{
	public class Supplier
	{
        public int Id { get; set; }
        public string CompanyName { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? PhoneNumber { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<Product>? Products { get; set; }
        public Supplier()
		{
		}
	}
}

