namespace SoftJail.DataProcessor
{

    using Data;
    using System;
    using System.Linq;
    using System.Text;

    public class Bonus
    {
        public static string ReleasePrisoner(SoftJailDbContext context, int prisonerId)
        {
            var prisoner = context.Prisoners.FirstOrDefault(x => x.Id == prisonerId);

            var sb = new StringBuilder();

            if (prisoner.ReleaseDate == null)
            {
                return $"Prisoner {prisoner.FullName} is sentenced to life";
            }
            else
            {
                prisoner.CellId = null;
                prisoner.ReleaseDate = DateTime.Now;
                context.SaveChanges();
                return $"Prisoner {prisoner.FullName} released";
            }

        }
    }
}
