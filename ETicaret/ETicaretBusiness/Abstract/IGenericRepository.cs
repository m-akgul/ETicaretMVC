using System;
using System.Linq.Expressions;

namespace ETicaretBusiness.Abstract
{
    public interface IGenericRepository<Tentity> where Tentity : class, new()
    {
        List<Tentity> GetAll(Expression<Func<Tentity, bool>> filter = null);
        List<Tentity> GetAll(Expression<Func<Tentity, bool>> filter = null, params Expression<Func<Tentity, object>>[] includes);
        Tentity Get(int id);
        Tentity Get(Expression<Func<Tentity, bool>> filter);
        Tentity Get(Expression<Func<Tentity, bool>> filter, params Expression<Func<Tentity, object>>[] includes);
        void Add(Tentity tentity);
        void Update(Tentity tentity);
        void Delete(Tentity tentity);
        void Delete(int id);
    }
}

