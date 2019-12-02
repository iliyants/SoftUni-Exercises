using FastFood.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Data
{
	public class FastFoodDbContext : DbContext
	{
		public FastFoodDbContext()
		{
		}

		public FastFoodDbContext(DbContextOptions options)
			: base(options)
		{
		}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Position> Positions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.Entity<OrderItem>(item =>
            {
                item.HasKey(x => new { x.ItemId, x.OrderId });

                item.HasOne(i => i.Item)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(i => i.ItemId);

                item.HasOne(o => o.Order)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(o => o.OrderId);
            });
		}
	}
}