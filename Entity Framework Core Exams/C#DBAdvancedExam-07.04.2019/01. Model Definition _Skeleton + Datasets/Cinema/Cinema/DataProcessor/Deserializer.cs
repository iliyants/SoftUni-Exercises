namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var movies = JsonConvert.DeserializeObject<Movie[]>(jsonString)
                .ToList();

            var sb = new StringBuilder();

            foreach (var movie in movies)
            {
                var movieExists = context.Movies.Any(x => x.Id == movie.Id);
                var validModel = IsValid(movie);
                var validEnum = Enum.TryParse(typeof(Genre), movie.Genre.ToString(), out object genre);

                if (movieExists || !validModel || !validEnum)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                context.Movies.Add(movie);
                sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, $"{movie.Rating:F2}"));

            }

            context.SaveChanges();

            return sb.ToString();

        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallsDTO = JsonConvert.DeserializeObject<ImportHallDTO[]>(jsonString)
                .ToArray();
              
            var sb = new StringBuilder();

            foreach (var hallDTO in hallsDTO)
            {
                var hallExist = context.Halls.Any(x => x.Name == hallDTO.Name);
                var validModel = IsValid(hallDTO);

                if (hallExist || !validModel)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentHall = new Hall
                {
                    Name = hallDTO.Name,
                    Is3D = hallDTO.Is3D,
                    Is4Dx = hallDTO.Is4Dx
                };

                for (int i = 0; i < hallDTO.Seats; i++)
                {
                    currentHall.Seats.Add(new Seat());
                }

                context.Halls.Add(currentHall);
                context.SaveChanges();

                var status = string.Empty;

                if(currentHall.Is3D && currentHall.Is4Dx)
                {
                    status = "4Dx/3D";
                }
                else if(!currentHall.Is3D && currentHall.Is4Dx)
                {
                    status = "4Dx";
                }
                else if(currentHall.Is3D && !currentHall.Is4Dx)
                {
                    status = "3D";
                }
                else
                {
                    status = "Normal";
                }

                sb.AppendLine(String
                    .Format(SuccessfulImportHallSeat, currentHall.Name,status, currentHall.Seats.Count()));
            }

            return sb.ToString();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var projectionsDTO = DeserializedCollection<ImportProjectionDTO>("Projections", xmlString)
                .ToArray();

            var sb = new StringBuilder();
            
            foreach (var projectionDTO in projectionsDTO)
            {
                var validHall = context.Halls.FirstOrDefault(x => x.Id == projectionDTO.HallId);
                var validMovie = context.Movies.FirstOrDefault(x => x.Id == projectionDTO.MovieId);
                var validModel = IsValid(projectionDTO);

                if (validHall == null || validMovie == null || !validModel)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var dateFormatParsed = DateTime
                        .ParseExact(projectionDTO.DateTime, "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture);

                var projection = new Projection()
                {
                    MovieId = projectionDTO.MovieId,
                    HallId = projectionDTO.HallId,
                    DateTime = dateFormatParsed
                };

                context.Projections.Add(projection);

                context.SaveChanges();

                var dateFormat =
                    projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                sb.AppendLine(string.Format(SuccessfulImportProjection, validMovie.Title, dateFormat));

            }

            return sb.ToString();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var customersDTO = DeserializedCollection<ImportCustomerDTO>("Customers", xmlString)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var customerDTO in customersDTO)
            {
                var validModel = IsValid(customerDTO);

                if (!validModel)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer()
                {
                    FirstName = customerDTO.FirstName,
                    LastName = customerDTO.LastName,
                    Age = customerDTO.Age,
                    Balance = customerDTO.Balance
                };

                var ticketsDTO = customerDTO.Tickets.ToArray();

                var tickets = Mapper.Map<ImportTicketDTO[], Ticket[]>(ticketsDTO);

                foreach (var ticket in tickets)
                {
                    customer.Tickets.Add(ticket);
                }

                context.Customers.Add(customer);

                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket,
                    customer.FirstName, customer.LastName, customer.Tickets.Count()));
            }

            context.SaveChanges();

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


    }
}