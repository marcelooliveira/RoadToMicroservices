using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Areas.Catalog.Models
{
    public class Category : BaseModel
    {
        public Category()
        {

        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }
    }
}
