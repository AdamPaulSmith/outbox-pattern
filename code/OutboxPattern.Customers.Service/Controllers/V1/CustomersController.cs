using System;
using Microsoft.AspNetCore.Mvc;
using OutboxPattern.Customers.Service.Model;

namespace OutboxPattern.Customers.Service.Controllers.V1
{
    [ApiController]
    [Route("/api/v1/{controller}")]
    public class CustomersController : Controller
    {
        [HttpGet("{customerId:Guid}")]
        public IActionResult Get(Guid customerId)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(AddCustomer addCustomer)
        {
            var customerId = Guid.NewGuid();

            return CreatedAtAction("Get", new {customerId}, new Customer
                                                            {
                                                                Id = customerId,
                                                                FirstName = addCustomer.FirstName,
                                                                LastName = addCustomer.LastName
                                                            });
        }
    }
}