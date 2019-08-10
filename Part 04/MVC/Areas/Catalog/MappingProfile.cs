using AutoMapper;

namespace MVC.Areas.Catalog
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<MVC.Models.Product, MVC.Areas.Catalog.Models.Product>();
            CreateMap<Areas.Catalog.Models.Product, MVC.Models.Product>();

            CreateMap<MVC.Models.Category, Areas.Catalog.Models.Category>();
            CreateMap<Areas.Catalog.Models.Category, MVC.Models.Category>();
        }
    }
}
