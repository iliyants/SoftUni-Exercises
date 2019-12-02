namespace MusicHub.DataProcessor
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
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writers = JsonConvert.DeserializeObject<Writer[]>(jsonString)
                .ToArray();

            var sb = new StringBuilder();


            foreach (var writer in writers)
            {
                var validModel = IsValid(writer);

                if (!validModel)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                context.Writers.Add(writer);

                context.SaveChanges();

                sb.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));
            }

            return sb.ToString();


        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producerAlbumsDTO = JsonConvert.DeserializeObject<ImportProducerAlbumsDTO[]>(jsonString)
                .ToArray();

            var sb = new StringBuilder();


            foreach (var producerDTO in producerAlbumsDTO)
            {
                
                var validModel = IsValid(producerDTO);
                var allValidAlbums = producerDTO.Albums.ToHashSet();
                var areValidAlbums = true;

                foreach (var isValidAlbum in allValidAlbums)
                {
                    if (!IsValid(isValidAlbum))
                    {
                        areValidAlbums = false;
                        break;
                    }

                }

                if (!validModel || !areValidAlbums)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //short way of validation
                //if (!IsValid(producerDTO) || !producerDTO.Albums.All(IsValid))
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                //using automapper instead of manually mapping the objects
                //var currentProducer = AutoMapper.Mapper.Map<Producer>(producerDTO);

                var currentProducer = new Producer()
                {
                    Name = producerDTO.Name,
                    Pseudonym = producerDTO.Pseudonym,
                    PhoneNumber = producerDTO.PhoneNumber
                };

                foreach (var album in producerDTO.Albums)
                {
                    var currentAlbum = new Album()
                    {
                        Name = album.Name,
                        ReleaseDate = DateTime
                        .ParseExact(album.ReleaseDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture)
                    };
                    currentProducer.Albums.Add(currentAlbum);
                }

                context.Producers.Add(currentProducer);

                context.SaveChanges();

                if (currentProducer.PhoneNumber == null)
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithNoPhone,
                        currentProducer.Name, currentProducer.Albums.Count()));
                }
                else
                {

                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone,
                        currentProducer.Name, currentProducer.PhoneNumber, currentProducer.Albums.Count()));
                }
            }
            return sb.ToString();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var songsDTO = DeserializedCollection<ImportSongDTO>("Songs", xmlString);

            var sb = new StringBuilder();

            var counter = 0;

            foreach (var songDTO in songsDTO)
            {
                var validModel = IsValid(songDTO);
                var validGenre = Enum.TryParse(songDTO.Genre, out Genre genreResult);
                var validWriter = context.Writers
                    .FirstOrDefault(x => x.Id == songDTO.WriterId);
                var validAlbum = context.Albums
                    .FirstOrDefault(x => x.Id == songDTO.AlbumId);

                if (!validModel || !validGenre || validWriter == null || validAlbum == null )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = AutoMapper.Mapper.Map<Song>(songDTO);
                counter++;

                context.Songs.Add(song);

                context.SaveChanges();

                sb.AppendLine(string.Format(SuccessfullyImportedSong,
                    song.Name, song.Genre, song.Duration));
            }

            Console.WriteLine(counter);

            return sb.ToString();
                
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var performersDTO = DeserializedCollection<ImportSongPerformerDTO>("Performers", xmlString);

            var sb = new StringBuilder();

            foreach (var performerDTO in performersDTO)
            {
                var validPerformer = IsValid(performerDTO);
                var validSongs = performerDTO.SongIds
                    .All(x => context.Songs.Any(y => x.Id == y.Id));

                if (!validPerformer || !validSongs)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var performer = AutoMapper.Mapper.Map<Performer>(performerDTO);

                foreach (var song in performerDTO.SongIds)
                {
                    var songPerformer = new SongPerformer()
                    {
                        PerformerId = performer.Id,
                        SongId = song.Id
                    };

                    performer.PerformerSongs.Add(songPerformer);
                }

                context.Performers.Add(performer);
                context.SaveChanges();

                sb.AppendLine(string.Format(
                    SuccessfullyImportedPerformer,
                    performer.FirstName, performer.PerformerSongs.Count()));
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
    }
}