using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> e)
        {
            e.HasKey(x => x.TownId);

            e.Property(x => x.Name)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(50);

            e.HasMany(x => x.Teams).WithOne(y => y.Town)
            .HasForeignKey(y => y.TownId);
        }
    }
}
