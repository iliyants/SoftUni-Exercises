namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public static class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(x => x.Rating >= rating &&
                       x.Projections.Any(t => t.Tickets.Count >= 1))
                .OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.Projections.Sum(y => y.Tickets.Sum(z => z.Price)))
                .Select(x => new ExportMovieDTO()
                {
                    MovieName = x.Title,
                    Rating = $"{x.Rating:F2}",
                    TotalIncomes = $"{x.Projections.Sum(y => y.Tickets.Sum(z => z.Price)):F2}",
                    Customers = x.Projections.SelectMany(y => y.Tickets.Select(t => new ExportCustomerDTO()
                    {
                        FirstName = t.Customer.FirstName,
                        LastName = t.Customer.LastName,
                        Balance = $"{t.Customer.Balance:F2}"
                    }))
                    .OrderByDescending(b => b.Balance)
                    .ThenBy(fn => fn.FirstName)
                    .ThenBy(ln => ln.LastName)
                    .ToArray()
                })
                .Take(10)
                .ToArray();

            var jsonString = JsonConvert.SerializeObject(movies, Newtonsoft.Json.Formatting.Indented);

            return jsonString;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers
                .Where(x => x.Age >= age)
                .OrderByDescending(x => x.Tickets.Sum(t => t.Price))
                .Select(x => new ExportCustomerSpendDTO()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SpentMoney = $"{x.Tickets.Sum(t => t.Price):F2}",
                    SpentTime = SumTimespans(x.Tickets.Select(t => t.Projection.Movie.Duration))

                })
                .Take(10)
                .ToArray();

            return SerializeCollectionToXML("Customers", customers);
        }

        public static string SerializeCollectionToXML<T>(string rootAttribute, T[] collection)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), collection, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string SerializeObjectToXML<T>(string rootAttribute, T currentObject)
        {
            var serializer = new XmlSerializer(typeof(T),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), currentObject, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string SumTimespans(this IEnumerable<TimeSpan> timeSpans)
        {
            TimeSpan sumTillNowTimeSpan = TimeSpan.Zero;

            foreach (TimeSpan timeSpan in timeSpans)
            {
                sumTillNowTimeSpan += timeSpan;
            }

            return sumTillNowTimeSpan.ToString();
        }
    }
}