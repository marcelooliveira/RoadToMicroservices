using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Category : BaseModel
    {
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }
    }
}
