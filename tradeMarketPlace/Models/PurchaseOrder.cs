namespace tradeMarketPlace.Models;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int BidId { get; set; }

    public int BuyerId { get; set; }

    public int SellerId { get; set; }

    public int Invoice { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public virtual Bid? Bid { get; set; } = null!;

    public virtual User? Buyer { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; } = null!;

    public virtual Rfp? QuantityNavigation { get; set; } = null!;

    public virtual User? Seller { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; } = null!;
}


public class PurchaseOrderPdf
{
    public int Id { get; set; }
    public decimal BidPrice { get; set; }
    public RfpPdf Rfp { get; set; }
    public User Seller { get; set; }
}