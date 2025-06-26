using Ecommerce.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ProductService.Data
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.ToTable("Products")
                .HasData(
                    new ProductModel { Id = 1, Name = "Laptop", Price = 100, Quantity = 50 },
                    new ProductModel { Id = 2, Name = "Smartphone", Price = 200, Quantity = 100 },
                    new ProductModel { Id = 3, Name = "Tablet", Price = 300, Quantity = 75 }
                );

            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ProductModel> Products { get; set; }
    }
}
