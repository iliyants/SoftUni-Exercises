namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;
    using PetClinic.Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext() { }

        public PetClinicContext(DbContextOptions options)
            :base(options) { }


        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalAid> AnimalAids { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }
        public DbSet<Vet> Vets { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Passport>(passport =>
            {
                passport.HasKey(x => x.SerialNumber);
            });

            builder.Entity<ProcedureAnimalAid>(pa =>
            {
                pa.HasKey(x => new { x.AnimalAidId, x.ProcedureId });

                pa.HasOne(x => x.AnimalAid)
                .WithMany(y => y.AnimalAidProcedures)
                .HasForeignKey(x => x.AnimalAidId);

                pa.HasOne(x => x.Procedure)
                .WithMany(y => y.ProcedureAnimalAids)
                .HasForeignKey(x => x.ProcedureId);
            });
        }
    }
}
