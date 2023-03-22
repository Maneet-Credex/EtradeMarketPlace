using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using tradeMarketPlace_Frontend.Models;

namespace tradeMarketPlace_Frontend.Controllers
{

    public class Messages : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;
        private readonly string _token;
        private readonly int _userId;
        public Messages(IHttpContextAccessor context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7014/api/");
            _token = _context.HttpContext.Session.GetString("token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            if (_context.HttpContext.Session.GetString("userId") != null)
            {

                _userId = int.Parse(_context.HttpContext.Session.GetString("userId"));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSenderChat([FromQuery] int ReceiverId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Messages/{_userId}/{ReceiverId}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<MessagesViewModel> messages = JsonConvert.DeserializeObject<List<MessagesViewModel>>(content);

                    ViewBag.SenderChatId = _userId;
                    ViewBag.ReceiverName = messages[0].ReceiverName;
                    ViewBag.messages = messages;
                    ViewBag.RecieverChatId = ReceiverId;
                    return View();
                }
                else
                {
                    ViewBag.SenderChatId = _userId;
                    ViewBag.ReceiverName = "test";
                    ViewBag.messages = null;
                    ViewBag.RecieverChatId = ReceiverId;
                    return View();
                }

            }
            catch (HttpRequestException ex)
            {
                // log the exception or handle it as appropriate
                throw new Exception("An error occurred while getting Messages.", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetSenderChat(int ReceiverId, MessagesViewModel model)
        {
            try
            {
                var messageData = new MessageJsonModel
                {
                    SenderId = _userId,
                    ReciverId = ReceiverId,
                    MsgContent = model.MessageContent,
                };
                var json = JsonConvert.SerializeObject(messageData);

                // Send POST request to API endpoint
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Get response content as string
                HttpResponseMessage response = await _httpClient.PostAsync("Messages", content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetSenderChat", "Messages", new { ReceiverId = ReceiverId });
                }
                else
                {
                    return View("Error");
                }

            }
            catch (HttpRequestException ex)
            {
                // log the exception or handle it as appropriate
                throw new Exception("An error occurred while getting Messages.", ex);
            }
        }
    }
}
