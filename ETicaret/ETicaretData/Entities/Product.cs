using System;
namespace ETicaretData.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public bool IsHome { get; set; }
        public bool IsApproved { get; set; }
        public int CategoryId { get; set; }
        public int SupplierID { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public int TotalSales { get; set; }

        public Product()
        {

        }
    }
}

