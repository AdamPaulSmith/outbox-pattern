using Microsoft.AspNetCore.Mvc;
using OutboxPattern.Customers.Service.Model;

namespace OutboxPattern.Customers.Service.Controllers.V1
{
    [ApiController]
    [Route("/api/v1/{controller}")]
    public class CustomersController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(AddCustomer addCustomer)
        {
            return Ok();
        }
    }
}