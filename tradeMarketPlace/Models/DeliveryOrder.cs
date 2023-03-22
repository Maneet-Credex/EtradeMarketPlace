using System;
using System.Collections.Generic;

namespace tradeMarketPlace.Models;

public partial class DeliveryOrder
{
    public int DeliveryOrderId { get; set; }

    public int Bid { get; set; }

    public int PoId { get; set; }

    public int Buyer { get; set; }

    public int Seller { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;

    public DateTime DeliveryDate { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime UpdatedOn { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public virtual Bid BidNavigation { get; set; } = null!;
}
