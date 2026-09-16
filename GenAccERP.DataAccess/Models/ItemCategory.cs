using System;
using System.Collections.Generic;

namespace GenAccERP.DataAccess.Models;

public partial class ItemCategory
{
    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual AppUser CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ItemMaster> ItemMasters { get; set; } = new List<ItemMaster>();

    public virtual AppUser? ModifiedByNavigation { get; set; }
}
