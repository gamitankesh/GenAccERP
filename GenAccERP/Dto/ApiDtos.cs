using System;
using System.Diagnostics.CodeAnalysis;

namespace GenAccERP.Dto
{
    [ExcludeFromCodeCoverage]
    public record UserDto
    (
        Guid UserId,
        string Username,
        string Email,
        string Role
    );

    [ExcludeFromCodeCoverage]
    public record CategoryDto
    (
        Guid CategoryId,
        string CategoryName,
        Guid CreatedBy,
        DateTime CreatedDate,
        UserDto? CreatedByUser
    );

    [ExcludeFromCodeCoverage]
    public record ItemDto
    (
        Guid ItemId,
        Guid CategoryId,
        string ItemNo,
        string ItemName,
        string? ItemDescription,
        decimal PurchasePrice,
        decimal SalesPrice,
        int CurrentStock,
        string? CategoryName,
        UserDto? CreatedByUser
    );
}

namespace GenAccERP.Dto
{
    [ExcludeFromCodeCoverage]
    public record PurchaseLineDto(Guid PurchaseDetailId, Guid ItemId, int Quantity, decimal UnitPrice, decimal? LineTotal);

    [ExcludeFromCodeCoverage]
    public record PurchaseDto(Guid PurchaseId, DateTime PurchaseDate, string? SupplierName, decimal TotalAmount, DateTime CreatedDate, System.Collections.Generic.List<PurchaseLineDto> Lines);

    [ExcludeFromCodeCoverage]
    public record SaleLineDto(Guid SalesDetailId, Guid ItemId, int Quantity, decimal UnitPrice, decimal? LineTotal);

    [ExcludeFromCodeCoverage]
    public record SaleDto(Guid SaleId, DateTime SaleDate, string? CustomerName, decimal TotalAmount, DateTime CreatedDate, System.Collections.Generic.List<SaleLineDto> Lines);

    [ExcludeFromCodeCoverage]
    public record StockDto(Guid ItemId, int Purchased, int Sold, int Available);
}
