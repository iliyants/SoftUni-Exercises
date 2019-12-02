using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P03_FootballBetting.Data.Configurations
{
    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> e)
        {
            e.HasKey(x => x.BetId);

            e.Property(x => x.Amount)
            .HasColumnName("money")
            .IsRequired();
        }
    }
}
