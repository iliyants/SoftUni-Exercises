using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> e)
        {
            e.HasKey(x => x.PlayerId);

            e.Property(x => x.Name)
            .IsUnicode(false)
            .IsRequired();
        }
    }
}
