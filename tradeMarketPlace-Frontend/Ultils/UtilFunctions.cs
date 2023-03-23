using Newtonsoft.Json;
using System.Net.Http.Headers;
using tradeMarketPlace_Frontend.Models;

namespace tradeMarketPlace_Frontend.Ultils
{
    public class UtilFunctions
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;
        private readonly string _token;

        public UtilFunctions(IHttpContextAccessor context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7014/api/");
            _token = _context.HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        //Function for getting all product categories
        public async Task<List<ProductCategoryViewModel>> GetProductCategoriesAsync()
        {

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("ProductCategories");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                List<ProductCategoryViewModel> categories = JsonConvert.DeserializeObject<List<ProductCategoryViewModel>>(content);

                return categories;
            }
            catch (HttpRequestException ex)
            {
                // log the exception or handle it as appropriate
                throw new Exception("An error occurred while getting the product categories.", ex);
            }
        }

        public async Task<List<RfpViewModel>> GetRfpAsync(int RfpId)
        {

            try
            {

                HttpResponseMessage response = await _httpClient.GetAsync($"Rfps/Rfp/{RfpId}");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                List<RfpViewModel> Rfp = JsonConvert.DeserializeObject<List<RfpViewModel>>(content);

                return Rfp;
            }
            catch (HttpRequestException ex)
            {
                // log the exception or handle it as appropriate
                throw new Exception("An error occurred while getting the product categories.", ex);
            }
        }
        public async Task<ExistingRfpBidViewModel> GetExistingRfpBid(int rfpId, int userId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await _httpClient.GetAsync($"Rfps/{rfpId}/{userId}");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                ExistingRfpBidViewModel rfpBid = JsonConvert.DeserializeObject<ExistingRfpBidViewModel>(content);

                return rfpBid;
            }
            catch (HttpRequestException ex)
            {
                // log the exception or handle it as appropriate
                throw new Exception("An error occurred while getting the RFP and Bid.", ex);
            }
        }

        public async Task<List<MessagesViewModel>> GetAllMessage(int senderId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Messages/{senderId}");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                List<MessagesViewModel> messages = JsonConvert.DeserializeObject<List<MessagesViewModel>>(content);

                return messages;
            }
            catch (HttpRequestException ex)
            {
                // log the exception or handle it as appropriate
                throw new Exception("An error occurred while getting Messages.", ex);
            }
        }

        public async Task<PurchaseOrderViewModel> GetBidRfpData(int bidId)
        {
            try
            {
                // Send the GET request and get the response
                HttpResponseMessage response = await _httpClient.GetAsync($"Bids/{bidId}");
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response to a list of Rfp objects
                PurchaseOrderViewModel rfpBid = JsonConvert.DeserializeObject<PurchaseOrderViewModel>(responseBody);
                return rfpBid;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting Messages.", ex);
            }
        }

    }
}
