using AutoMapper;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models;

namespace FastFood.App
{
	public class FastFoodProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public FastFoodProfile()
		{
            CreateMap<Category, ExportCategoryDTO>();
		}
	}
}
