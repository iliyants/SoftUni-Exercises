namespace VaporStore.DataProcessor
{
    using System;
    using Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using System.Collections.Generic;
    using VaporStore.DataProcessor.ExportDTOs;
    using System.Globalization;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var result = new List<ExportGenreDTO>();

            foreach (var genre in genreNames)
            {
                var currentGenre = context.Genres
                    .Where(x => x.Name == genre)
                .Select(x => new ExportGenreDTO
                {
                    Id = x.Id,
                    Genre = x.Name,

                    Games = x.Games
                    .Where(p => p.Purchases.Any())
                    .Select(g => new ExportGameDTO
                    {
                        Id = g.Id,
                        Title = g.Name,
                        Developer = g.Developer.Name,
                        Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name).ToArray()),
                        Players = g.Purchases.Count()
                    })
                    .OrderByDescending(g => g.Players)
                    .ThenBy(i => i.Id)
                    .ToArray(),

                    TotalPlayers = x.Games.Sum(y => y.Purchases.Count())

                }).FirstOrDefault();


                result.Add(currentGenre);
            }

            var jsonString = JsonConvert.SerializeObject(
                result
                .OrderByDescending(x => x.Games.Sum(p => p.Players))
                .ThenBy(x => x.Id), Newtonsoft.Json.Formatting.Indented);

            return jsonString;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {

            var users = context.Users
                      .Select(x => new ExportUserPurchaseDTO
                      {
                          Username = x.Username,
                          Purchases = x.Cards
                          .SelectMany(p => p.Purchases)
                          .Where(p => p.Type.ToString() == storeType)
                          .Select(y => new ExportPurchaseDTO
                          {
                              CardNumber = y.Card.Number,
                              Cvc = y.Card.Cvc,
                              Date = y.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                              Game = new ExportGamePriceDTO
                              {
                                  Title = y.Game.Name,
                                  Genre = y.Game.Genre.Name.ToString(),
                                  Price = y.Game.Price
                              }
                          })
                          .OrderBy(d => d.Date)
                          .ToArray(),

                          TotalSpent = x.Cards.SelectMany(c => c.Purchases)
                                    .Where(p => p.Type.ToString() == storeType)
                                    .Sum(p => p.Game.Price)

                      })
                      .Where(p => p.Purchases.Any())
                      .OrderByDescending(x => x.TotalSpent)
                      .ThenBy(x => x.Username)
                      .ToArray();

            return SerializeCollectionToXML("Users", users);

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
    }
}