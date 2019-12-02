using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> e)
        {
            e.HasKey(x => x.UserId);

            e.Property(x => x.Username)
            .HasMaxLength(20)
            .IsUnicode(false)
            .IsRequired();

            e.Property(x => x.Password)
           .HasMaxLength(20)
           .IsUnicode(false)
           .IsRequired();

            e.HasMany(x => x.Bets).WithOne(y => y.User)
            .HasForeignKey(y => y.UserId);
        }
    }
}
