using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GenAccERP.DataAccess.Models;

[ExcludeFromCodeCoverage]
public partial class SalesMaster
{
    public Guid SaleId { get; set; }

    public DateTime SaleDate { get; set; }

    public string? CustomerName { get; set; }

    public decimal TotalAmount { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual AppUser CreatedByNavigation { get; set; } = null!;

    public virtual AppUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
}
