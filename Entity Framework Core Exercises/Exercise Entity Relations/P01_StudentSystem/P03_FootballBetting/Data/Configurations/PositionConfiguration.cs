using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> e)
        {
            e.HasKey(x => x.PositionId);

            e.Property(x => x.Name)
            .IsUnicode(false)
            .IsRequired();

            e.HasMany(x => x.Players).WithOne(y => y.Position)
            .HasForeignKey(y => y.PositionId);
        }
    }
}
