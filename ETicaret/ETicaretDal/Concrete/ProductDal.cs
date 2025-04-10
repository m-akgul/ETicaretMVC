using System;
using ETicaretBusiness.Concrete;
using ETicaretDal.Abstract;
using ETicaretData.Context;
using ETicaretData.Entities;

namespace ETicaretDal.Concrete
{
	public class ProductDal : GenericRepository<Product, ETicaretContext>, IProductDal
    {
		public ProductDal()
		{
		}
	}
}

