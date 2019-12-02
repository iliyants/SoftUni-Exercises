using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data
{
    public class SalesContext:DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=Sales;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurateProductsDescriptionDefaultValue(modelBuilder);
            ConfigurateSaleDateDefaultValue(modelBuilder);
        }

        private void ConfigurateSaleDateDefaultValue(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(s =>
            {
                s.Property(d => d.Date).HasDefaultValueSql("getdate()");
            });
        }

        private void ConfigurateProductsDescriptionDefaultValue(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(p =>
            {
                p.Property(d => d.Description).HasDefaultValue("No Description");
            });
        }
    }
}
