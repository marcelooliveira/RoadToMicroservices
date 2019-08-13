using AutoMapper;

namespace MVC.Areas.Catalog
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Models.Product, API.Catalog.Models.Product>();
            CreateMap<API.Catalog.Models.Product, Models.Product>();

            CreateMap<Models.Category, API.Catalog.Models.Category>();
            CreateMap<API.Catalog.Models.Category, Models.Category>();
        }
    }
}
