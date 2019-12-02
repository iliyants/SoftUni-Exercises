using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> e)
        {
            e.HasKey(x => x.CountryId);

            e.Property(x => x.Name)
            .HasMaxLength(20)
            .IsUnicode()
            .IsRequired();

            e.HasMany(x => x.Towns).WithOne(y => y.Country)
            .HasForeignKey(y => y.CountryId);
        }
    }
}
