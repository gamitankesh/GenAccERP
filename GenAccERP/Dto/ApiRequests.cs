using System;
using System.Diagnostics.CodeAnalysis;

namespace GenAccERP.Dto
{
    [ExcludeFromCodeCoverage]
    public class UserRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = null!;
        public Guid? CreatedBy { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CreateItemRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? ItemNo { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public int CurrentStock { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class UpdateItemRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? ItemNo { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PurchaseRequest
    {
        public Guid? ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? SupplierName { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SaleRequest
    {
        public Guid? ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? SaleDate { get; set; }
        public string? CustomerName { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
