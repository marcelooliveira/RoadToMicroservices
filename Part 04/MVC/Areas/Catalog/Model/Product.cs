using Nest;

namespace MVC.Areas.Catalog.Models
{
    [ElasticsearchType(IdProperty = nameof(Code))]
    public class Product : BaseModel
    {
        public Category Category { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get { return $"/images/catalog/large_{Code}.jpg"; } }

        public Product()
        {

        }

        public Product(int id, string code, string name, decimal price, Category category)
        {
            Id = id;
            Code = code;
            Name = name;
            Price = price;
            Category = category;
        }
    }
}
