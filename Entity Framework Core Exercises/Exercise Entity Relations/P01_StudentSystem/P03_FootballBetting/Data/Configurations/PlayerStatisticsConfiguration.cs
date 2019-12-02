using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class PlayerStatisticsConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> e)
        {
            e.HasKey(x => new
            {
                x.PlayerId,
                x.GameId
            });

            e.Property(x => x.ScoredGoals)
            .HasDefaultValue(0);

            e.Property(x => x.Assists)
            .HasDefaultValue(0);


            e.Property(x => x.MinutesPlayed)
            .HasDefaultValue(0);

            e.HasOne(x => x.Game).WithMany(y => y.PlayerStatistics)
            .HasForeignKey(x => x.GameId);

            e.HasOne(x => x.Player).WithMany(y => y.PlayerStatistics)
            .HasForeignKey(x => x.PlayerId);
        }
    }
}
