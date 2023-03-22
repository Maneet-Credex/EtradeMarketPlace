namespace tradeMarketPlace.Models;

public partial class Bid
{
    public int BidId { get; set; }

    public int RfpId { get; set; }

    public int UserId { get; set; }

    public decimal Price { get; set; }

    public DateTime BidDateTime { get; set; }

    public string Comments { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime UpdateOn { get; set; }

    public int UpdatedBy { get; set; }

    public int CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DeliveryOrder> DeliveryOrders { get; } = new List<DeliveryOrder>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; } = new List<PurchaseOrder>();

    public virtual Rfp? Rfp { get; set; } = null;

    public virtual User? UpdatedByNavigation { get; set; } = null!;

    public virtual User? User { get; set; } = null!;


}

public class BidsForRfpModel
{
    public int BidId { get; set; }
    public int RFPId { get; set; }
    public decimal Price { get; set; }
    public DateTime Date { get; set; }
    public string Comments { get; set; }
    public string OrgatisationName { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string RfpName { get; set; }
}
