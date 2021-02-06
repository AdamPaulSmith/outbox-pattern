using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using OutboxPattern.Customers.Service;
using OutboxPattern.Customers.Service.Model;
using OutboxPattern.Customers.Tests.Integration.ResponseModels;
using Shouldly;
using Xbehave;
using Xunit;

namespace OutboxPattern.Customers.Tests.Integration.Customers
{
    public class AddingCustomers : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _webApplicationFactory;

        public AddingCustomers(CustomWebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Scenario]
        public void CustomersFirstNameNotPopulatedResultsInValidationError(AddCustomer addCustomer, HttpResponseMessage httpResponseMessage)
        {
            "If the customers FirstName property is not populated"
                .x(_ =>
                   {
                       addCustomer = new AddCustomer
                                     {
                                         FirstName = string.Empty,
                                         LastName = "Test"
                                     };
                   });
            "When the Customer is posted to the Customers Controller"
                .x(async _ =>
                   {
                       httpResponseMessage = await _webApplicationFactory.CreateClient()
                                                                         .PostAsJsonAsync("/api/v1/customers",
                                                                                          addCustomer);
                   });

            "Then a Bed Request response is sent back with a descriptive error message"
                .x(async _ =>
                   {
                       httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
                       var responseContent = await httpResponseMessage.Content.ReadFromJsonAsync<Http400StatusResponseModel>();
                       responseContent.ShouldNotBeNull();
                       responseContent.Title.ShouldBe("One or more validation errors occurred.");
                       responseContent.Errors.ShouldNotBeNull();
                       responseContent.Errors.ShouldContainKey("FirstName");
                       string[] fieldValidationErrors = responseContent.Errors["FirstName"];
                       fieldValidationErrors.ShouldNotBeNull();
                       fieldValidationErrors.Length.ShouldBe(1);
                       fieldValidationErrors[0].ShouldBe("The FirstName field is required.");
                   });
        }

        [Scenario]
        public void CustomersLastNameNotPopulatedResultsInValidationError(AddCustomer addCustomer, HttpResponseMessage httpResponseMessage)
        {
            "If the customers FirstName property is not populated"
                .x(_ =>
                   {
                       addCustomer = new AddCustomer
                                     {
                                         FirstName = "Test",
                                         LastName = string.Empty
                                     };
                   });
            "When the Customer is posted to the Customers Controller"
                .x(async _ =>
                   {
                       httpResponseMessage = await _webApplicationFactory.CreateClient()
                                                                         .PostAsJsonAsync("/api/v1/customers",
                                                                                          addCustomer);
                   });

            "Then a Bed Request response is sent back with a descriptive error message"
                .x(async _ =>
                   {
                       httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
                       var responseContent = await httpResponseMessage.Content.ReadFromJsonAsync<Http400StatusResponseModel>();
                       responseContent.ShouldNotBeNull();
                       responseContent.Title.ShouldBe("One or more validation errors occurred.");
                       responseContent.Errors.ShouldNotBeNull();
                       responseContent.Errors.ShouldContainKey("LastName");
                       string[] fieldValidationErrors = responseContent.Errors["LastName"];
                       fieldValidationErrors.ShouldNotBeNull();
                       fieldValidationErrors.Length.ShouldBe(1);
                       fieldValidationErrors[0].ShouldBe("The LastName field is required.");
                   });
        }
    }
}