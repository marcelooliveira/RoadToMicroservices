namespace API.Catalog.Models
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
