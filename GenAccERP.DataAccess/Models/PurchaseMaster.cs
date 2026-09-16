using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GenAccERP.DataAccess.Models;

[ExcludeFromCodeCoverage]
public partial class PurchaseMaster
{
    public Guid PurchaseId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string? SupplierName { get; set; }

    public decimal TotalAmount { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual AppUser CreatedByNavigation { get; set; } = null!;

    public virtual AppUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
}
