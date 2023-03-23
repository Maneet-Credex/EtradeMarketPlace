namespace tradeMarketPlace_Frontend.Models
{
    public class BidsViewModel
    {
        public int BidId { get; set; }

        public int RfpId { get; set; }

        public int UserId { get; set; }

        public decimal Price { get; set; }

        public DateTime BidDateTime { get; set; } = DateTime.Now;

        public string Comments { get; set; } = null!;

        public string Status { get; set; } = "Valid";

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime UpdateOn { get; set; } = DateTime.Now;

        public int UpdatedBy { get; set; }

        public int CreatedBy { get; set; }
    }

    public class BidsForRfpViewModel
    {
        public int BidId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public string organisationName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RfpName { get; set; }
    }
}
