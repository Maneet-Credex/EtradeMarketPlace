using System.Net.Http.Headers;
using System.Text;

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
        public async Task ChangeRfpStatusToClosed(int rfpId)
        {
            // Code to make API call to change RFP status to closed

            using (var client = new HttpClient())
            {
                // Set the API endpoint URL
                var apiUrl = $"https://localhost:7014/api/rfps/status/{rfpId}";

                // Create the request content (if any)
                var requestContent = new StringContent("", Encoding.UTF8, "application/json");

                // Add any required headers (e.g. authentication token)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "<your-authentication-token>");

                // Make the API call
                var response = await client.PutAsync(apiUrl, requestContent);

                // Check if the API call was successful
                if (!response.IsSuccessStatusCode)
                {
                    // Handle error
                    throw new Exception("Failed to change RFP status to closed.");
                }
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

    public class RfpPdfViewModel
    {

        public string Title { get; set; }
        public int RfpId { get; set; }
        public int RfpQuantity { get; set; }
        public decimal RfpPrice { get; set; }
        public DateTime RfpLastDate { get; set; }
        public string Description { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public UserViewModel Buyer { get; set; }

    }
}
