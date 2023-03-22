using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using tradeMarketPlace_Frontend.Models;

namespace tradeMarketPlace_Frontend.Controllers
{
    public class DashBoardRFP : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;
        private readonly string _token;
        private readonly int _userId;
        private readonly string _userRole;
        public DashBoardRFP(IHttpContextAccessor context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7014/api/");
            _token = _context.HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _userId = int.Parse(_context.HttpContext.Session.GetString("userId"));
            _userRole = _context.HttpContext.Session.GetString("role");

        }
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


    }
}

