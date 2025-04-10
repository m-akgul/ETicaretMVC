using System;
using ETicaretBusiness.Concrete;
using ETicaretDal.Abstract;
using ETicaretData.Context;
using ETicaretData.Entities;

namespace ETicaretDal.Concrete
{
	public class CategoryDal : GenericRepository<Category, ETicaretContext>, ICategoryDal
    {
		public CategoryDal()
		{
		}
	}
}

