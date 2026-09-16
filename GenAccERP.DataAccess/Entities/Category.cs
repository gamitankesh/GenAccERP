using System.Collections.Generic;

namespace GenAccERP.DataAccess.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
