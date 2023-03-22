using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using tradeMarketPlace_Frontend.Models;
using tradeMarketPlace_Frontend.Ultils;

namespace tradeMarketPlace_Frontend.Controllers
{
    public class DashBoardSeller : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UtilFunctions _utilFuntions;

        public DashBoardSeller(IHttpContextAccessor context, IHttpClientFactory httpClientFactory, UtilFunctions utilFunctions)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _utilFuntions = utilFunctions;
        }

        [HttpGet]
        public async Task<IActionResult> Seller()
        {
            var httpContextAccessor = HttpContext.RequestServices.GetService<IHttpContextAccessor>();

            try
            {
                var client = new HttpClient();

                string token = _context.HttpContext.Session.GetString("token");
                string userRole = _context.HttpContext.Session.GetString("role");
                string userName = _context.HttpContext.Session.GetString("userName");
                int userId = Int32.Parse(_context.HttpContext.Session.GetString("userId"));
                // Set the base address of the API
                if (userRole == null || userRole != "seller")
                {
                    return RedirectToAction("Login", "Account");
                }
                client.BaseAddress = new Uri("https://localhost:7014/");

                // Set the JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Send the GET request and get the response
                var userRfp = await client.GetAsync($"api/Rfps");
                var productCategories = await _utilFuntions.GetProductCategoriesAsync();
                ExistingRfpBidViewModel rfpBid = await _utilFuntions.GetExistingRfpBid(7, userId);
                RfpViewModel rfp = rfpBid.Rfp;
                BidsViewModel bid = rfpBid.Bid;
                var messages = await _utilFuntions.GetAllMessage(userId);

                if (userRfp.IsSuccessStatusCode)
                {

                    // Read the response content as a string
                    string responseBody = await userRfp.Content.ReadAsStringAsync();

                    // Parse the JSON response to a list of Rfp objects
                    List<RfpViewModel> rfpData = JsonConvert.DeserializeObject<List<RfpViewModel>>(responseBody);
                    var progressPercentage = new RfpProgressViewModel();

                    if (messages.Count() > 0)
                    {
                        ViewBag.Messages = messages;
                    }
                    else
                    {
                        ViewBag.Messages = null;
                    }
                    ViewBag.HttpContextAccessor = httpContextAccessor;
                    ViewBag.existingRfp = rfp;
                    ViewBag.existingBid = bid;
                    ViewBag.DataPP = progressPercentage;
                    ViewBag.DataPc = productCategories;
                    ViewBag.rfpData = rfpData;
                    return View();
                }
                else
                {
                    string errorMessage = await userRfp.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        public IActionResult AddBid([FromQuery] int RfpId)
        {
            var model = new BidsViewModel
            {
                RfpId = RfpId
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddBid([FromQuery] int RfpId, BidsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();
                string token = _context.HttpContext.Session.GetString("token");
                int UserId = int.Parse(_context.HttpContext.Session.GetString("userId"));

                // Set the base address of the API
                httpClient.BaseAddress = new Uri("https://localhost:7014/");

                // Set the JWT token in the Authorization header
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var bidData = new BidsViewModel
                {
                    RfpId = model.RfpId,
                    UserId = UserId,
                    BidDateTime = DateTime.Now,
                    Price = model.Price,
                    Comments = model.Comments,
                    CreationDate = model.CreationDate,
                    UpdateOn = model.UpdateOn,
                    UpdatedBy = UserId,
                    CreatedBy = UserId,
                    Status = model.Status,
                };

                var json = JsonConvert.SerializeObject(bidData);

                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send POST request to API endpoint
                var response = await httpClient.PostAsync("api/Bids", content);

                // Get response content as string
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("seller", "DashBoardSeller");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating bid. Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            // Add a return statement here to handle the case where ModelState is valid
            return RedirectToAction("seller", "DashBoardeller");
        }

        [HttpGet]
        public async Task<IActionResult> EditBid([FromQuery] int BidId)
        {
            try
            {
                var client = new HttpClient();

                string token = _context.HttpContext.Session.GetString("token");
                string userRole = _context.HttpContext.Session.GetString("role");
                string userName = _context.HttpContext.Session.GetString("userName");
                int userId = Int32.Parse(_context.HttpContext.Session.GetString("userId"));
                // Set the base address of the API
                if (userRole == null || userRole != "seller")
                {
                    return RedirectToAction("Login", "Account");
                }
                client.BaseAddress = new Uri("https://localhost:7014/");

                // Set the JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Send the GET request and get the response
                var userBid = await client.GetAsync($"api/Bids/{BidId}");

                if (userBid.IsSuccessStatusCode)

                {

                    // Read the response content as a string
                    string responseBody = await userBid.Content.ReadAsStringAsync();

                    // Parse the JSON response to a list of Bid objects
                    BidsViewModel BidData = JsonConvert.DeserializeObject<BidsViewModel>(responseBody);


                    return View(BidData);
                }
                else
                {
                    // Handle the error response
                    // For example, you can read the error message as a string:
                    string errorMessage = await userBid.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBid(int BidId, BidsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();
                string token = _context.HttpContext.Session.GetString("token");
                int UserId = int.Parse(_context.HttpContext.Session.GetString("userId"));

                httpClient.BaseAddress = new Uri("https://localhost:7014/"); // base address of your API
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var bidData = new BidsViewModel
                {
                    BidId = BidId,
                    UserId = UserId,
                    RfpId = model.RfpId,
                    BidDateTime = DateTime.Now,
                    Price = model.Price,
                    Comments = model.Comments,
                    CreationDate = model.CreationDate,
                    UpdateOn = model.UpdateOn,
                    UpdatedBy = UserId,
                    CreatedBy = UserId,
                    Status = model.Status,
                };

                var json = JsonConvert.SerializeObject(bidData);

                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send PUT request to API endpoint with the RFP ID in the URL
                var response = await httpClient.PutAsync($"api/Bids/{BidId}", content);

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("seller", "DashBoardSeller");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating. Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            return RedirectToAction("seller", "DashBoardSeller");
        }

        public IActionResult RfpDetail([FromQuery] int RfpId)
        {
            return View();
        }



    }
}
