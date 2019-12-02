namespace MusicHub
{
    using AutoMapper;
    using MusicHub.Data.Models;
    using MusicHub.DataProcessor.ImportDtos;
    using System;
    using System.Globalization;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            CreateMap<ImportProducerAlbumsDTO, Producer>();
            CreateMap<ImportAlbumDTO, Album>()
                .ForMember(x => x.ReleaseDate, y => y.MapFrom(d => DateTime
                       .ParseExact(d.ReleaseDate, "dd/MM/yyyy",
                       CultureInfo.InvariantCulture)));

            CreateMap<ImportSongDTO, Song>()
                .ForMember(x => x.CreatedOn, y => y.MapFrom
                (c => DateTime.ParseExact(c.CreatedOn, @"dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.Duration, y => y.MapFrom
                (d => TimeSpan.ParseExact(d.Duration, @"hh\:mm\:ss", CultureInfo.InvariantCulture)));

            CreateMap<ImportSongPerformerDTO, Performer>()
                .ForMember(x => x.PerformerSongs, y => y.Ignore());
            

        }
    }
}
