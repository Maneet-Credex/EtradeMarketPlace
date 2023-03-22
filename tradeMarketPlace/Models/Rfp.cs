namespace tradeMarketPlace.Models;

public partial class Rfp
{
    public int RfpId { get; set; }

    public int UserId { get; set; }

    public int ProductCategoryId { get; set; }
    public int ProductSubCategoryId { get; set; }
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal MaxPrice { get; set; }

    public string Status { get; set; } = "open";
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime UpdateOn { get; set; } = DateTime.Now;

    public string RfpName { get; set; }
    public string RfpDescription { get; set; }
    public DateTime LastDate { get; set; }

    public int UpdatedBy { get; set; }

    public int CreatedBy { get; set; }


    public virtual ICollection<Bid>? Bids { get; } = new List<Bid>();

    public virtual User? CreatedByNavigation { get; set; } = null!;

    public virtual ProductCategory? ProductCategory { get; set; } = null!;

    public virtual ProductCatalogue? Product { get; set; } = null!;

    public virtual SubCategory? ProductSubCategory { get; set; } = null!;

    public virtual ICollection<PurchaseOrder>? PurchaseOrders { get; } = new List<PurchaseOrder>();

    public virtual User? UpdatedByNavigation { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
