using System.Collections.Generic;

namespace MVC.Areas.Catalog.Models.ViewModels
{
    public class SearchProductsViewModel
    {
        public string SearchText { get; set; }
        public List<Product> Products { get; set; }
    }
}
