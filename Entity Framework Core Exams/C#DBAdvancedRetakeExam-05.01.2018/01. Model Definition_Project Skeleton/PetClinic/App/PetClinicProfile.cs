namespace PetClinic.App
{
    using AutoMapper;
    using PetClinic.DataProcessor.ImportDTOs;
    using PetClinic.Models;
    using System;
    using System.Globalization;

    public class PetClinicProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public PetClinicProfile()
        {
            CreateMap<ImportAnimalAidDTO, AnimalAid>();

            CreateMap<ImportAnimalDTO, Animal>()
                .ForMember(x => x.Passport, p => p.Ignore());

            CreateMap<ImportPassportDTO, Passport>()
                .ForMember(x => x.RegistrationDate,
                rd => rd.MapFrom(d =>
                DateTime.ParseExact(d.RegistrationDate, "dd-mm-yyyy", CultureInfo.InvariantCulture)));

            CreateMap<ImportVetDTO, Vet>();
        }
    }
}
