using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project02_ApiConsumeUI.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace NetCoreAI.Project02_ApiConsumeUI.Controllers
{
    public class CustomerController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: CustomerController
        public async Task<ActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyClient");
            var response = await client.GetAsync("Customer");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<CustomerDto>>(jsonData);
                return View(values);
            }
            else
            {
                // Handle error response
                ModelState.AddModelError(string.Empty, "Unable to retrieve customers.");
                return View(new List<CustomerDto>());
            }
        }

        // GET: CustomerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("MyClient");
            var response = await client.GetAsync($"Customer/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<CustomerDto>(jsonData);
                return View(values);
            }
            return View();
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCustomerDto createCustomerDto)
        {
            var client = _httpClientFactory.CreateClient("MyClient");
            var jsonData = JsonConvert.SerializeObject(createCustomerDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("Customer", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: CustomerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("MyClient");
            var response = await client.GetAsync($"Customer/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<CustomerDto>(jsonData);
                return View(values);
            }
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerDto customerDto)
        {
            var client = _httpClientFactory.CreateClient("MyClient");
            var jsonData = JsonConvert.SerializeObject(customerDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync($"Customer/{customerDto.CustomerID}", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("MyClient");
            var responseMessage = await client.DeleteAsync($"Customer/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
