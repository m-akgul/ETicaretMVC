using System;
using ETicaretData.Entities;
using ETicaretData.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ETicaretData.Context
{
    public class ETicaretContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ETicaretContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=MERTVICTUS\\SQLEXPRESS;Database=ETicaret;Trusted_Connection=True;");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}

