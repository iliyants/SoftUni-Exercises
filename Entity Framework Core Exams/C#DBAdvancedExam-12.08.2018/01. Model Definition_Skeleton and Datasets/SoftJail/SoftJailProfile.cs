namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Globalization;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            CreateMap<ImportDepartmentDTO, Department>();
            CreateMap<ImportCellDTO, Cell>();

            CreateMap<ImportPrisonerDTO, Prisoner>()
                .ForMember(x => x.IncarcerationDate,
                y => y.MapFrom(d => DateTime.ParseExact(d.IncarcerationDate, "dd/MM/yyyy",
                CultureInfo.InvariantCulture)))
                .ForMember(x => x.ReleaseDate,
                y => y.MapFrom(d => DateTime.ParseExact(d.ReleaseDate, "dd/MM/yyyy",
                CultureInfo.InvariantCulture)));
            CreateMap<ImportMailDTO, Mail>();

            CreateMap<ImportOfficerDTO, Officer>()
                .ForMember(x => x.OfficerPrisoners, y => y.Ignore());

        }


    }
}
