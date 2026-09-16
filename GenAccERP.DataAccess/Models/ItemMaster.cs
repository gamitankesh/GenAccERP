using System;
using System.Collections.Generic;

namespace GenAccERP.DataAccess.Models;

public partial class ItemMaster
{
    public Guid ItemId { get; set; }

    public Guid CategoryId { get; set; }

    public string ItemNo { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string? ItemDescription { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal SalesPrice { get; set; }

    public int CurrentStock { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ItemCategory Category { get; set; } = null!;

    public virtual AppUser CreatedByNavigation { get; set; } = null!;

    public virtual AppUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
}
