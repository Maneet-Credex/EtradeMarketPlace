namespace tradeMarketPlace_Frontend.Models
{
    public class RfpViewModel

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

        public DateTime LastDate { get; set; }

        public int UpdatedBy { get; set; }

        public int CreatedBy { get; set; }

        public string? RfpName { get; set; }

        public string? RfpDescription { get; set; }
    }
    public class ProgressInfo
    {
        public int DaysLeft { get; set; }
        public int Percentage { get; set; }
    }

    public class RfpProgressViewModel
    {
        public DateTime CreatedDate { get; set; }
        public DateTime LastDate { get; set; }

        public ProgressInfo ProgressPercentage
        {
            get
            {
                var totalDays = (LastDate - CreatedDate).TotalDays;
                var elapsedDays = (DateTime.UtcNow - CreatedDate).TotalDays;
                var daysLeft = Convert.ToInt32(totalDays - elapsedDays);
                var percentage = Convert.ToInt32((elapsedDays / totalDays) * 100);
                if (percentage < 0) percentage = 0;
                else if (percentage > 100) percentage = 100;

                return new ProgressInfo
                {
                    DaysLeft = daysLeft,
                    Percentage = percentage
                };
            }
        }
    }

    public class RfpViewData
    {
        public string Name { get; set; }
        public List<RfpViewModel> Rfps { get; set; }
        public RfpProgressViewModel Progress { get; set; }
        public List<ProductCategoryViewModel> ProductCategories { get; set; }
        //public List<RfpViewModel> rfpModel { get; set; }
        // public RfpViewModel rfpModel { get; set; }
    }
}
