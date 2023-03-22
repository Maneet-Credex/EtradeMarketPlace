using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using tradeMarketPlace_Frontend.Models;
using tradeMarketPlace_Frontend.Ultils;

namespace tradeMarketPlace_Frontend.Controllers
{
    public class DashBoardBuyer : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UtilFunctions _utilFuntions;

        public DashBoardBuyer(IHttpContextAccessor context, IHttpClientFactory httpClientFactory, UtilFunctions utilFunctions)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _utilFuntions = utilFunctions;
        }

        [HttpGet]
        public async Task<IActionResult> Buyer()
        {
            try
            {
                var client = new HttpClient();

                string token = _context.HttpContext.Session.GetString("token");
                string userRole = _context.HttpContext.Session.GetString("role");
                string userName = _context.HttpContext.Session.GetString("userName");
                int userId = Int32.Parse(_context.HttpContext.Session.GetString("userId"));
                // Set the base address of the API
                if (userRole == null || userRole != "buyer")
                {
                    return RedirectToAction("Login", "Account");
                }
                client.BaseAddress = new Uri("https://localhost:7014/");

                // Set the JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Send the GET request and get the response
                var productCategories = await _utilFuntions.GetProductCategoriesAsync();
                var messages = await _utilFuntions.GetAllMessage(userId);
                if (productCategories.Count() > 0)
                {
                    ViewBag.DataPc = productCategories;
                }
                else
                {
                    ViewBag.DataPc = null;
                }

                if (messages.Count() > 0)
                {
                    ViewBag.Messages = messages;
                }
                else
                {
                    ViewBag.Messages = null;
                }

                var userRfp = await client.GetAsync($"api/Rfps/{userId}");
                if (userRfp.IsSuccessStatusCode)
                {

                    // Read the response content as a string
                    string responseBody = await userRfp.Content.ReadAsStringAsync();

                    // Parse the JSON response to a list of Rfp objects
                    List<RfpViewModel> rfpData = JsonConvert.DeserializeObject<List<RfpViewModel>>(responseBody);
                    var progressPercentage = new RfpProgressViewModel();


                    ViewBag.DataPP = progressPercentage;

                    ViewBag.rfpData = rfpData;
                    return View();
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Buyer(RfpViewModel model)
        {

            if (ModelState.IsValid)

            {
                var httpClient = _httpClientFactory.CreateClient();
                string token = _context.HttpContext.Session.GetString("token");
                int UserId = int.Parse(_context.HttpContext.Session.GetString("userId"));

                // Set the base address of the API


                // Set the JWT token in the Authorization header
                httpClient.BaseAddress = new Uri("https://localhost:7014/"); // base address of your API
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var rfpData = new RfpViewModel
                {
                    RfpName = model.RfpName,
                    UserId = UserId,
                    Quantity = model.Quantity,
                    MaxPrice = model.MaxPrice,
                    ProductCategoryId = model.ProductCategoryId,
                    ProductSubCategoryId = model.ProductSubCategoryId,
                    ProductId = model.ProductId,
                    CreationDate = model.CreationDate,
                    UpdateOn = model.UpdateOn,
                    UpdatedBy = UserId,
                    CreatedBy = UserId,
                    RfpDescription = model.RfpDescription,
                    LastDate = model.LastDate,
                    Status = model.Status,
                };

                var json = JsonConvert.SerializeObject(rfpData);

                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send POST request to API endpoint
                var response = await httpClient.PostAsync("api/Rfps", content);

                // Get response content as string
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("buyer", "DashBoardBuyer");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while registering. Please try again.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            // Add a return statement here to handle the case where ModelState is valid
            return RedirectToAction("buyer", "DashBoardBuyer");
        }

        [HttpGet]
        public async Task<IActionResult> EditRfp([FromQuery] int rfpid)
        {
            try
            {
                var client = new HttpClient();

                string token = _context.HttpContext.Session.GetString("token");
                string userRole = _context.HttpContext.Session.GetString("role");
                string userName = _context.HttpContext.Session.GetString("userName");
                int userId = Int32.Parse(_context.HttpContext.Session.GetString("userId"));
                // Set the base address of the API
                if (userRole == null || userRole != "buyer")
                {
                    return RedirectToAction("Login", "Account");
                }
                client.BaseAddress = new Uri("https://localhost:7014/");

                // Set the JWT token in the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Send the GET request and get the response
                var userRfp = await client.GetAsync($"api/Rfps/rfp/{rfpid}");
                var productCategories = await _utilFuntions.GetProductCategoriesAsync();

                if (userRfp.IsSuccessStatusCode)
                {

                    // Read the response content as a string
                    string responseBody = await userRfp.Content.ReadAsStringAsync();

                    // Parse the JSON response to a list of Rfp objects
                    List<RfpViewModel> rfpData = JsonConvert.DeserializeObject<List<RfpViewModel>>(responseBody);
                    var progressPercentage = new RfpProgressViewModel();

                    ViewBag.DataPc = productCategories;
                    ViewBag.rfpData = rfpData;
                    return View(rfpData[0]);
                }
                else
                {
                    // Handle the error response
                    // For example, you can read the error message as a string:
                    string errorMessage = await userRfp.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRfp(int rfpid, RfpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();
                string token = _context.HttpContext.Session.GetString("token");
                int UserId = int.Parse(_context.HttpContext.Session.GetString("userId"));

                httpClient.BaseAddress = new Uri("https://localhost:7014/"); // base address of your API
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var rfpData = new RfpViewModel
                {
                    RfpId = model.RfpId,
                    UserId = UserId,
                    RfpName = model.RfpName,
                    Quantity = model.Quantity,
                    MaxPrice = model.MaxPrice,
                    ProductCategoryId = model.ProductCategoryId,
                    ProductSubCategoryId = model.ProductSubCategoryId,
                    ProductId = model.ProductId,
                    UpdateOn = DateTime.Now,
                    UpdatedBy = UserId,
                    CreatedBy = model.CreatedBy,
                    RfpDescription = model.RfpDescription,
                    LastDate = model.LastDate,
                };

                var json = JsonConvert.SerializeObject(rfpData);

                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send PUT request to API endpoint with the RFP ID in the URL
                var response = await httpClient.PutAsync($"api/Rfps/{rfpid}", content);

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("buyer", "DashBoardBuyer");
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

            return RedirectToAction("buyer", "DashBoardBuyer");
        }


        public async Task<IActionResult> RfpDetail([FromQuery] int RfpId)
        {
            List<RfpViewModel> rfpDetails = await _utilFuntions.GetRfpAsync(RfpId);
            return View(rfpDetails[0]);
        }

    }
}
