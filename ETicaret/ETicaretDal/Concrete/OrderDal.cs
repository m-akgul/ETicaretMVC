using System;
using ETicaretBusiness.Concrete;
using ETicaretDal.Abstract;
using ETicaretData.Context;
using ETicaretData.Entities;

namespace ETicaretDal.Concrete
{
	public class OrderDal : GenericRepository<Order, ETicaretContext>, IOrderDal
    {
		public OrderDal()
		{
		}
	}
}

