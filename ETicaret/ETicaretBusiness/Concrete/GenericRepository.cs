using System;
using System.Linq.Expressions;
using ETicaretBusiness.Abstract;
using ETicaretData.Context;
using ETicaretData.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ETicaretBusiness.Concrete
{
    public class GenericRepository<Tentity, Tcontext> : IGenericRepository<Tentity>
        where Tentity : class, new()
        where Tcontext : IdentityDbContext<AppUser, AppRole, int>, new()
    {
        public GenericRepository()
        {
        }

        public void Add(Tentity tentity)
        {
            using (var db = new Tcontext())
            {
                db.Set<Tentity>().Add(tentity);
                db.SaveChanges();
            }
        }

        public void Delete(Tentity tentity)
        {
            using (var db = new Tcontext())
            {
                db.Entry(tentity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new Tcontext())
            {
                var nesne = db.Set<Tentity>().Find(id);
                db.Set<Tentity>().Remove(nesne);
                db.SaveChanges();
            }
        }

        public Tentity Get(int id)
        {
            using (var db = new Tcontext())
            {
                var nesne = db.Set<Tentity>().Find(id);
                return nesne;
            }
        }

        public Tentity Get(Expression<Func<Tentity, bool>> filter)
        {
            using (var db = new Tcontext())
            {
                var nesne = db.Set<Tentity>().Find(filter);
                return nesne;
            }
        }

        public Tentity Get(Expression<Func<Tentity, bool>> filter, params Expression<Func<Tentity, object>>[] includes)
        {
            using (var db = new Tcontext())
            {
                IQueryable<Tentity> query = db.Set<Tentity>();

                // Eğer Include'lar varsa, hepsini ekle
                if (includes.Length > 0)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return query.FirstOrDefault(filter); // İlk eşleşen kaydı getir
            }
        }

        public List<Tentity> GetAll(Expression<Func<Tentity, bool>> filter = null)
        {
            using (var db = new Tcontext())
            {
                return filter == null ? db.Set<Tentity>().ToList() : db.Set<Tentity>().Where(filter).ToList();
            }
        }

        public List<Tentity> GetAll(Expression<Func<Tentity, bool>> filter = null, params Expression<Func<Tentity, object>>[] includes)
        {
            using (var db = new Tcontext())
            {
                IQueryable<Tentity> query = db.Set<Tentity>();

                // Eğer bir filtre gönderildiyse uygula
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Include işlemlerini ekle
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return query.ToList();
            }
        }

        public void Update(Tentity tentity)
        {
            using (var db = new Tcontext())
            {
                db.Entry(tentity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}

