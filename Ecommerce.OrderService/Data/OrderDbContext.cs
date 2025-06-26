using Ecommerce.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.OrderService.Data
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderModel>(entity =>
            {
                entity.ToTable("Orders")
                .HasData(
                    new OrderModel { Id = 1, CustomerName = "Laptop", ProductId=1, Quantity = 10, OrderDate =DateTime.Now },
                    new OrderModel { Id = 2, CustomerName = "Smartphone", ProductId = 2, Quantity = 20, OrderDate = DateTime.Now },
                    new OrderModel { Id = 3, CustomerName = "Tablet", ProductId = 3, Quantity = 30, OrderDate = DateTime.Now }
                );

            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<OrderModel> Orders { get; set; } = null!;
    }
}
