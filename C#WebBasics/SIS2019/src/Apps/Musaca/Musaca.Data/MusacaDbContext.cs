using Microsoft.EntityFrameworkCore;
using Musaca.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Musaca.Data
{
    public class MusacaDbContext:DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ReceiptOrder> ReceiptsOrders { get; set; }

        public DbSet<ProductOrder> ProducsOrders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($@"Integrated Security=True;Server=.\SQLEXPRESS;Database=MusacaDb");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(e =>
            {
                e.HasMany(x => x.Receipts)
                .WithOne(y => y.Cashier)
                .HasForeignKey(x => x.CashierId);

                e.HasMany(x => x.Orders)
                .WithOne(y => y.Cashier)
                .HasForeignKey(x => x.CashierId);
            });

            modelBuilder.Entity<ProductOrder>(e =>
            {
                e.HasKey(x => new { x.OrderId, x.ProductId });
            });

            modelBuilder.Entity<ReceiptOrder>(e =>
            {
                e.HasKey(x => new { x.ReceiptId, x.OrderId });
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.HasMany(x => x.ProductOrders)
                .WithOne(y => y.Product)
                .HasForeignKey(x => x.ProductId);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.HasMany(x => x.OrderProducts)
                .WithOne(y => y.Order)
                .HasForeignKey(x => x.OrderId);

                e.HasMany(x => x.OrderReceipts)
                .WithOne(y => y.Order)
                .HasForeignKey(x => x.OrderId);
            });

            modelBuilder.Entity<Receipt>(e =>
            {
                e.HasMany(x => x.ReceiptOrders)
                .WithOne(y => y.Receipt)
                .HasForeignKey(x => x.ReceiptId);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
