namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Enums;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDTOs;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDTO = JsonConvert.DeserializeObject<GameDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var gameDTO in gamesDTO)
            {
                var validGame = IsValid(gameDTO);
                var missingTags = gameDTO.Tags.Count() == 0;
                var emptyTags = gameDTO.Tags.Any(x => x == "" || string.IsNullOrEmpty(x));

                if (!validGame || missingTags || emptyTags)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var developer = context.Developers.FirstOrDefault(x => x.Name == gameDTO.Developer)
                     ?? CreateDeveloper(gameDTO.Developer, context);
                var genre = context.Genres.FirstOrDefault(x => x.Name == gameDTO.Genre)
                    ?? CreateGenre(gameDTO.Genre, context);
                var tags = GetTags(gameDTO.Tags, context);

                var game = new Game()
                {
                    Name = gameDTO.Name,
                    Price = gameDTO.Price,
                    ReleaseDate = Convert.ToDateTime(gameDTO.ReleaseDate, CultureInfo.InvariantCulture),
                    Developer = developer,
                    Genre = genre
                };

                foreach (var tag in tags)
                {
                    var gameTag = new GameTag()
                    {
                        Tag = tag,
                        Game = game
                    };

                    game.GameTags.Add(gameTag);
                }
                context.Games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count()} tags");

            }

            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDTO = JsonConvert.DeserializeObject<UserDTO[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var user in usersDTO)
            {
                var validUser = IsValid(user);
                var validCards = user.Cards.All(IsValid);
                var validTypes = user.Cards.Any(x => Enum.TryParse(typeof(CardType), x.Type, out object type));

                if (!validUser || !validCards || !validTypes)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var currentUser = AutoMapper.Mapper.Map<User>(user);

                context.Users.Add(currentUser);

                sb.AppendLine($"Imported {currentUser.Username} with {currentUser.Cards.Count()} cards");

            }
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var purchasesDTO = DeserializedCollection<PurchaseDTO>("Purchases", xmlString);

            var sb = new StringBuilder();

            foreach (var purchaseDTO in purchasesDTO)
            {
                var validPurchase = IsValid(purchasesDTO);
                var validPurchaseType = Enum.TryParse(typeof(PurchaseType), purchaseDTO.PurchaseType, out object type);
                var game = context.Games.FirstOrDefault(x => x.Name == purchaseDTO.GameTitle);

                if (!validPurchase || !validPurchaseType || game == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var card = context.Cards.FirstOrDefault(x => x.Number == purchaseDTO.CardNumber);

                var parsedDate = DateTime
                    .ParseExact(purchaseDTO.Date,
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

                var purchase = new Purchase()
                {
                    Game = game,
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDTO.PurchaseType, true),
                    ProductKey = purchaseDTO.ProductKey,
                    Card = card,
                    Date = parsedDate
                };

                context.Purchases.Add(purchase);
                context.SaveChanges();
                var user = context
                    .Users
                    .Where(x => x.Cards.Any(c => c.Number == purchaseDTO.CardNumber))
                    .Select(x => x.Username)
                    .FirstOrDefault();
                    
                sb.AppendLine($"Imported {purchaseDTO.GameTitle} for {user}");

            }

            return sb.ToString();
        }

        private static bool IsValid(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(model, validationContext, validationResult, true);
        }

        public static T[] DeserializedCollection<T>(string rootAttribute, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            T currentClass = (T)Activator.CreateInstance(typeof(T));

            var typeDeserialization = (T[])serializer
            .Deserialize(new StringReader(inputXml));

            return typeDeserialization;
        }


        private static List<Tag> GetTags(string[] tags, VaporStoreDbContext context)
        {
            var result = new List<Tag>();
            foreach (var currentTag in tags)
            {
                var tag = context.Tags.FirstOrDefault(x => x.Name == currentTag)
                    ?? CreateTag(currentTag, context);

                result.Add(tag);
            }
            context.SaveChanges();
            return result;
        }

        private static Tag CreateTag(string tag, VaporStoreDbContext context)
        {
            var currentTag = new Tag()
            {
                Name = tag
            };

            context.Tags.Add(currentTag);
            context.SaveChanges();
            return currentTag;
        }

        private static Genre CreateGenre(string genre, VaporStoreDbContext context)
        {
            var newGenre = new Genre()
            {
                Name = genre
            };

            context.Genres.Add(newGenre);
            context.SaveChanges();
            return newGenre;

        }

        private static Developer CreateDeveloper(string developer, VaporStoreDbContext context)
        {
            var newDeveloper = new Developer()
            {
                Name = developer
            };
            context.Developers.Add(newDeveloper);
            context.SaveChanges();
            return newDeveloper;
        }
    }
}