using System.Collections.Generic;

namespace MVC.Models.ViewModels
{
    public class CarouselPageViewModel
    {
        public CarouselPageViewModel()
        {

        }

        public CarouselPageViewModel(List<Product> products, int pageIndex)
        {
            Products = products;
            PageIndex = pageIndex;
        }

        public List<Product> Products { get; set; }
        public int PageIndex { get; set; }
    }
}
