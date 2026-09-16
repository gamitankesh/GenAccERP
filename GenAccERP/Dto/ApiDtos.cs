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
