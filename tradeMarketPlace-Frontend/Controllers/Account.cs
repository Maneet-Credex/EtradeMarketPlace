using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using tradeMarketPlace.Models;
using tradeMarketPlace_Frontend.Models;

namespace tradeMarketPlace_Frontend.Controllers
{

    public class Account : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _context;

        public Account(IHttpClientFactory httpClientFactory, IHttpContextAccessor context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri("https://localhost:7014/"); // base address of your API

                var user = new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    organisationName = model.OrganisationName,
                    email = model.Email,
                    password = model.Password,
                    type = model.Type,
                    contactNumber = model.ContactNumber,
                    status = model.Status
                };

                var response = await httpClient.PostAsJsonAsync("api/Users/Register", user);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while registering. Please try again.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri("https://localhost:7014/"); // base address of your API

                var url = $"api/Users/Login?email={model.Email}&password={model.Password}";
                var response = await httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UserLoginResponse>(json);
                    if (result.IsSuccess)
                    {
                        if (result.data.Role == null)
                        {
                            return Unauthorized();
                        }
                        // success, get the token from the response
                        var token = result.token;

                        // save the token in a session for future use
                        _context.HttpContext.Session.SetString("token", token);
                        _context.HttpContext.Session.SetString("userId", result.data.UserID);
                        _context.HttpContext.Session.SetString("userEmail", result.data.UserEmail);
                        _context.HttpContext.Session.SetString("userName", result.data.UserName);
                        _context.HttpContext.Session.SetString("role", result.data.Role);
                        // ...

                        // redirect to the dashboard
                        if (result.data.Role == "buyer")
                        {
                            return RedirectToAction(result.data.Role, "DashBoardBuyer");

                        }
                        else if (result.data.Role == "admin")
                        {

                            return RedirectToAction(result.data.Role, "DashBoardAdmin");
                        }
                        else
                        {
                            return RedirectToAction(result.data.Role, "DashBoardSeller");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect email or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while registering. Please try again.");
                }
            }
            // Add an error message to the ModelState if login fails

            // Return the view with the updated ModelState
            return View(model);
        }


        public IActionResult Logout()
        {
            _context.HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}