using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> e)
        {
            e.HasKey(x => x.TeamId);

            e.Property(x => x.Name)
            .HasMaxLength(100)
            .IsUnicode()
            .IsRequired();

            e.Property(x => x.LogoUrl)
           .HasMaxLength(100)
           .IsUnicode()
           .IsRequired();

            e.Property(x => x.Initials)
           .HasMaxLength(10)
           .IsUnicode()
           .IsRequired();

            e.Property(x => x.Budget)
            .IsRequired();

            e.HasMany(x => x.HomeGames).WithOne(y => y.HomeTeam)
            .HasForeignKey(y => y.HomeTeamId);

            e.HasMany(x => x.AwayGames).WithOne(y => y.AwayTeam)
           .HasForeignKey(y => y.AwayTeamId);

            e.HasMany(x => x.Players).WithOne(y => y.Team)
            .HasForeignKey(y => y.TeamId);
        }
    }
}
