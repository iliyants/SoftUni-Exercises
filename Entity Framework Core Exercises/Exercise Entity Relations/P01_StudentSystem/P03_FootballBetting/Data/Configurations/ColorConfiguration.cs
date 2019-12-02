using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class ColorConfiguration :IEntityTypeConfiguration<Color>
    {
     
       public void Configure(EntityTypeBuilder<Color> e)
        {
            e.HasKey(x => x.ColorId);

            e.Property(x => x.Name)
            .HasMaxLength(20)
            .IsUnicode()
            .IsRequired();

            e.HasMany(x => x.PrimaryKitTeams).WithOne(y => y.PrimaryKitColor)
                    .HasForeignKey(y => y.PrimaryKitColorId);

            e.HasMany(x => x.SecondaryKitTeams).WithOne(y => y.SecondaryKitColor)
                    .HasForeignKey(y => y.SecondaryKitColorId);
        }
    }
}
