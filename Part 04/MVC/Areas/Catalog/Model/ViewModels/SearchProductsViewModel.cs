using System.Collections.Generic;

namespace MVC.Areas.Catalog.Models.ViewModels
{
    public class SearchProductsViewModel
    {
        public SearchProductsViewModel(List<Product> products, string searchText)
        {
            Products = products;
            SearchText = searchText;
        }

        public List<Product> Products { get; set; }
        public string SearchText { get; set; }
    }
}
