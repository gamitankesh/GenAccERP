using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GenAccERP.DataAccess.Models;

[ExcludeFromCodeCoverage]
public partial class PurchaseDetail
{
    public Guid PurchaseDetailId { get; set; }

    public Guid PurchaseId { get; set; }

    public Guid ItemId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual ItemMaster Item { get; set; } = null!;

    public virtual PurchaseMaster Purchase { get; set; } = null!;
}
