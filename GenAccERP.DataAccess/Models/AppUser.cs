using System;
using System.Collections.Generic;

namespace GenAccERP.DataAccess.Models;

public partial class AppUser
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual AppUser? CreatedByNavigation { get; set; }

    public virtual ICollection<AppUser> InverseCreatedByNavigation { get; set; } = new List<AppUser>();

    public virtual ICollection<AppUser> InverseModifiedByNavigation { get; set; } = new List<AppUser>();

    public virtual ICollection<ItemCategory> ItemCategoryCreatedByNavigations { get; set; } = new List<ItemCategory>();

    public virtual ICollection<ItemCategory> ItemCategoryModifiedByNavigations { get; set; } = new List<ItemCategory>();

    public virtual ICollection<ItemMaster> ItemMasterCreatedByNavigations { get; set; } = new List<ItemMaster>();

    public virtual ICollection<ItemMaster> ItemMasterModifiedByNavigations { get; set; } = new List<ItemMaster>();

    public virtual AppUser? ModifiedByNavigation { get; set; }

    public virtual ICollection<PurchaseMaster> PurchaseMasterCreatedByNavigations { get; set; } = new List<PurchaseMaster>();

    public virtual ICollection<PurchaseMaster> PurchaseMasterModifiedByNavigations { get; set; } = new List<PurchaseMaster>();

    public virtual ICollection<SalesMaster> SalesMasterCreatedByNavigations { get; set; } = new List<SalesMaster>();

    public virtual ICollection<SalesMaster> SalesMasterModifiedByNavigations { get; set; } = new List<SalesMaster>();
}
