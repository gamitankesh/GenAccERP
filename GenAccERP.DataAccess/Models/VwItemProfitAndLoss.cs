using System;
using System.Collections.Generic;

namespace GenAccERP.DataAccess.Models;

public partial class VwItemProfitAndLoss
{
    public Guid ItemId { get; set; }

    public string ItemNo { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public int TotalPurchasedQty { get; set; }

    public decimal TotalPurchaseCost { get; set; }

    public int TotalSoldQty { get; set; }

    public decimal TotalSalesRevenue { get; set; }

    public int CurrentStock { get; set; }

    public decimal? EstimatedProfit { get; set; }
}
