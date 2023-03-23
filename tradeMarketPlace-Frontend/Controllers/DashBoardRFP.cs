using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using tradeMarketPlace_Frontend.Models;
using tradeMarketPlace_Frontend.Ultils;

namespace tradeMarketPlace_Frontend.Controllers
{
    public class DashBoardRFP : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;
        private readonly string _token;
        private readonly int _userId;
        private readonly string _userRole;
        private readonly UtilFunctions _utilFuntions;
        public DashBoardRFP(IHttpContextAccessor context, HttpClient httpClient, UtilFunctions utilFunctions)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7014/api/");
            _token = _context.HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _userId = int.Parse(_context.HttpContext.Session.GetString("userId"));
            _userRole = _context.HttpContext.Session.GetString("role");
            _utilFuntions = utilFunctions;

        }
        [HttpGet]
        public async Task<IActionResult> RFP([FromQuery] int rfpId)

        {
            try
            {
                if (_userRole == null || _userRole != "buyer")
                {
                    return RedirectToAction("Login", "Account");
                }

                // Send the GET request and get the response
                var RfpBids = await _httpClient.GetAsync($"Bids/BidsForRfp/{rfpId}");

                if (RfpBids.IsSuccessStatusCode)
                {

                    // Read the response content as a string
                    string responseBody = await RfpBids.Content.ReadAsStringAsync();

                    // Parse the JSON response to a list of Rfp objects
                    List<BidsForRfpViewModel> BidData = JsonConvert.DeserializeObject<List<BidsForRfpViewModel>>(responseBody);
                    return View(BidData);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RFP([FromQuery] int bidId, PurchaseOrderViewModel model)
        {
            try
            {
                if (_userRole == null || _userRole != "buyer")
                {
                    return RedirectToAction("Login", "Account");
                }

                var BidRfpData = await _utilFuntions.GetBidRfpData(bidId);

                var json = JsonConvert.SerializeObject(BidRfpData);

                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send POST request to API endpoint
                var Pdfresponse = await _httpClient.PostAsync("PurchaseOrders/generatepdf", content);

                // Get response content as string
                var responseString = await Pdfresponse.Content.ReadAsStringAsync();

                if (Pdfresponse.IsSuccessStatusCode)
                {
                    var requestContent = new StringContent("", Encoding.UTF8, "application/json");


                    // Make the API call
                    var response = await _httpClient.PutAsync($"rfps/status/{BidRfpData.Rfp.RfpId}", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("RFP", "DashBoardRFP");
                    }
                    else
                    {
                        return BadRequest($"Status response: {response}");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while registering. Please try again.");
                }

                return View();


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

