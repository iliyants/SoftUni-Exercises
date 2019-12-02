namespace BookShop
{
    using System;
    using Data;
    using System.Linq;
    using Initializer;
    using BookShop.Models.Enums;
    using System.Text;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new BookShopContext())
            {

                //use method here
            }
        }



        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context
                .Books
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {

            var books = context.Books
                .Where(x => x.Copies < 5000 && x.EditionType.ToString() == "Gold")
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new
                {
                    x.Title,
                    x.Price
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => DateTime.Parse(x.ReleaseDate.ToString()).Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var books = context.Books
                .Where(x => x.BookCategories
                .Any(y => categories.Select(c => c.ToLower())
                .Contains(y.Category.Name.ToLower())))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var beforeDate = DateTime
                .ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate < beforeDate)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    Title = x.Title,
                    Edition = x.EditionType,
                    Price = x.Price
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.Edition} - ${book.Price:F2}");
            }

            return sb.ToString();
                
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.ToLower().EndsWith(input.ToLower()))
                .Select(x => new
                {
                    FullName = $"{x.FirstName} {x.LastName}"
                })
                .OrderBy(x => x.FullName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName}");
            }

            return sb.ToString();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    Title = x.Title,
                    AuthorFullName = $"{x.Author.FirstName} {x.Author.LastName}"
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorFullName})");
            }

            return sb.ToString();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .Select(x => x.Title)
                .ToList();

            return books.Count;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors

                .Select(x => new
                {
                    AuthorFullName = $"{x.FirstName} {x.LastName}",
                    TotalCopies = x.Books.Sum(c => c.Copies)
                })
                .OrderByDescending(x => x.TotalCopies)
                .ToDictionary(x => x.AuthorFullName, x => x.TotalCopies);

            var sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.Key} - {author.Value}");
            }

            return sb.ToString();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    Name = x.Name,
                    TotalProfit =
                    x.CategoryBooks.Sum(y => y.Book.Copies * y.Book.Price)
                })
                .OrderByDescending(x => x.TotalProfit)
                .ThenBy(x => x.Name);

            return string.Join
                (Environment.NewLine, categories.Select(x => $"{x.Name} ${x.TotalProfit}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    Name = x.Name,
                    MostRecentBooks = 
                    x.CategoryBooks
                        .OrderByDescending(y => y.Book.ReleaseDate)
                        .Select(y => new
                        {
                            Title = y.Book.Title,
                            Year = y.Book.ReleaseDate.Value.Year
                        })
                        .Take(3)
                        .ToList()
                                               
                })
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Name,y => y.MostRecentBooks);

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Key}");
                foreach (var book in category.Value)
                {
                    sb.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return sb.ToString();

        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToDelete = context.Books.Where(x => x.Copies < 4200).ToList();
            context.Books.RemoveRange(booksToDelete);

            context.SaveChanges();

            return booksToDelete.Count();
        }
    }
}
