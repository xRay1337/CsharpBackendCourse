using System.Collections.Generic;

namespace ShopEf.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();

        public Category()
        {
        }

        public Category(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Id.ToString().PadLeft(3, '0')} {Name}";
        }
    }
}