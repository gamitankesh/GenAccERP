using System.ComponentModel.DataAnnotations.Schema;

namespace GenAccERP.DataAccess.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        // FK to Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
