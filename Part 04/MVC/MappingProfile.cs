using AutoMapper;
using MVC.Areas.Catalog.Models;
using MVC.Models;

namespace MVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Models.Product, Areas.Catalog.Models.Product>();
            CreateMap<Areas.Catalog.Models.Product, Models.Product>();

            CreateMap<Models.Category, Areas.Catalog.Models.Category>();
            CreateMap<Areas.Catalog.Models.Category, Models.Category>();
        }
    }
}
