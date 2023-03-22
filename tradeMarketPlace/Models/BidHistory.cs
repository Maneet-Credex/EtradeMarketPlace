using System;
using System.Collections.Generic;

namespace tradeMarketPlace.Models;

public partial class BidHistory
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public int BidId { get; set; }

    public int UserId { get; set; }
}
