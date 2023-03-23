namespace tradeMarketPlace_Frontend.Models
{

    public class PurchaseOrderViewModel
    {
        public int Id { get; set; }
        public decimal BidPrice { get; set; }
        public RfpPdfViewModel Rfp { get; set; }
        public UserViewModel Seller { get; set; }
    }
}
