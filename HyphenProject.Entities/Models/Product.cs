using HyphenProject.Core.Entities;

namespace HyphenProject.Entities.Models
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
    }
}
