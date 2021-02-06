using Microsoft.AspNetCore.Mvc;

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
    }
}