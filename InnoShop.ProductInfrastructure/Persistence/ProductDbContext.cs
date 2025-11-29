    using InnoShop.ProductDomain.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace InnoShop.ProductInfrastructure.Persistence
    {
        public class ProductDbContext : DbContext
        {
            public ProductDbContext(DbContextOptions<ProductDbContext> options)
                : base(options)
            {
            }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Product>().HasKey(p => p.Id);

                base.OnModelCreating(modelBuilder);
            }
        }
    }
