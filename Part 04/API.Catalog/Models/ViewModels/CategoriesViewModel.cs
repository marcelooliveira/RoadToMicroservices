using System.Collections.Generic;

namespace API.Catalog.Models.ViewModels
{
    public class CategoriesViewModel
    {
        public CategoriesViewModel()
        {

        }

        public CategoriesViewModel(List<Category> categories, List<Product> products, int pageSize)
        {
            Categories = categories;
            Products = products;
            PageSize = pageSize;
        }

        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public int PageSize { get; set; }
    }
}
