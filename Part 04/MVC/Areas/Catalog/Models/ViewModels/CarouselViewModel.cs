using MVC.Models;
using System.Collections.Generic;

namespace MVC.Areas.Catalog.Models.ViewModels
{
    public class CarouselViewModel
    {
        public CarouselViewModel()
        {

        }

        public CarouselViewModel(Category category, List<Product> products, int pageCount, int pageSize)
        {
            Category = category;
            Products = products;
            PageCount = pageCount;
            PageSize = pageSize;
        }

        public Category Category { get; set; }
        public List<Product> Products { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
    }
}
