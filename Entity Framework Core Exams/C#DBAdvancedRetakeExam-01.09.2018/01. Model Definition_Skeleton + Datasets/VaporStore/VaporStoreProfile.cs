namespace VaporStore
{
	using AutoMapper;
    using System;
    using VaporStore.Data.Enums;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDTOs;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
		{
            CreateMap<UserDTO, User>();
            CreateMap<CardDTO, Card>();

		}
	}
}