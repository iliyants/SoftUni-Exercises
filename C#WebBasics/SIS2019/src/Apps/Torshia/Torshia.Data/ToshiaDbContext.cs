using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Torshia.Models;

namespace Torshia.Data
{
    public class ToshiaDbContext:DbContext
    {
        
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DBConfiguration.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersTasks>(e =>
            {
                e.HasKey(key => new { key.TaskId, key.UserId });

                e.HasOne(x => x.User)
                .WithMany(y => y.UserTasks)
                .HasForeignKey(x => x.UserId);

                e.HasOne(x => x.Task)
                .WithMany(y => y.TaskUsers)
                .HasForeignKey(x => x.TaskId);
            });

            modelBuilder.Entity<TasksSectors>(e =>
            {
                e.HasKey(x => new { x.SectorId, x.TaskId });

                e.HasOne(x => x.Sector)
                .WithMany(y => y.SectorTasks)
                .HasForeignKey(x => x.SectorId);

                e.HasOne(x => x.Task)
                .WithMany(y => y.AffectedSectors)
                .HasForeignKey(x => x.TaskId);
            });
         

            base.OnModelCreating(modelBuilder);
        }
    }
}
