using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GenAccERP.DataAccess.Models;

[ExcludeFromCodeCoverage]
public partial class SalesDetail
{
    public Guid SalesDetailId { get; set; }

    public Guid SaleId { get; set; }

    public Guid ItemId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual ItemMaster Item { get; set; } = null!;

    public virtual SalesMaster Sale { get; set; } = null!;
}
