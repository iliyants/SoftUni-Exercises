using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> e)
        {
            e.HasKey(x => x.GameId);

            e.HasMany(x => x.Bets).WithOne(y => y.Game)
            .HasForeignKey(y => y.GameId);
        }
    }
}
