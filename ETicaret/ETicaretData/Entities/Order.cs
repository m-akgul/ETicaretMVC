using System;
using ETicaretData.ViewModels;

namespace ETicaretData.Entities
{
	public class Order
	{
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public EnumOrderState orderState { get; set; }
        public string UserName { get; set; }
        public string AddressTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string? ApprovedBy { get; set; }
        public virtual List<OrderLine>? OrderLines { get; set; }

        public Order()
        {

        }
    }
}

