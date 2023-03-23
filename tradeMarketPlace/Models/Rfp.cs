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

public class RfpPdf
{

    public string Title { get; set; }
    public int RfpID { get; set; }
    public int RfpQuantity { get; set; }
    public decimal RfpPrice { get; set; }
    public DateTime RfpLastDate { get; set; }
    public string Description { get; set; }
    public string ProductCategory { get; set; }
    public string ProductSubCategory { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public User Buyer { get; set; }

}