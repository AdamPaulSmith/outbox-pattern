using System;
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
        public void CustomersLastNameNotPopulatedResultsInValidationError(AddCustomer addCustomer, HttpResponseMessage httpResponseMessage)
        {
            "If the customers FirstName property is not populated"
               .x(stepContext =>
                  {
                      addCustomer = new AddCustomer
                                    {
                                        FirstName = "TestFirstName",
                                        LastName = string.Empty
                                    };
                  });
            
            "When the Customer is posted to the Customers Controller"
               .x(async stepContext =>
                  {
                      httpResponseMessage = await _webApplicationFactory.CreateClient()
                                                                        .PostAsJsonAsync("/api/v1/customers",
                                                                                         addCustomer);
                  });

            "Then a Bed Request response is sent back with a descriptive error message"
               .x(async stepContext =>
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

        [Scenario]
        public void CustomersFirstNameNotPopulatedResultsInValidationError(AddCustomer addCustomer, HttpResponseMessage httpResponseMessage)
        {
            "If the customers FirstName property is not populated"
               .x(stepContext =>
                  {
                      addCustomer = new AddCustomer
                                    {
                                        FirstName = string.Empty,
                                        LastName = "TestLastName"
                                    };
                  });
            "When the Customer is posted to the Customers Controller"
               .x(async stepContext =>
                  {
                      httpResponseMessage = await _webApplicationFactory.CreateClient()
                                                                        .PostAsJsonAsync("/api/v1/customers",
                                                                                         addCustomer);
                  });

            "Then a Bed Request response is sent back with a descriptive error message"
               .x(async stepContext =>
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
        public void CustomerWithValidInputsReturnsCorrectlyFormattedLocationHeader(AddCustomer addCustomer, HttpResponseMessage httpResponseMessage)
        {
            "If the customers properties are populated correctly"
               .x(stepContext =>
                  {
                      addCustomer = new AddCustomer
                                    {
                                        FirstName = "TestFirstName",
                                        LastName = "TestLastName"
                                    };
                  });

            "When the Customer is posted to the Customers Controller"
               .x(async stepContext =>
                  {
                      httpResponseMessage = await _webApplicationFactory.CreateClient()
                                                                        .PostAsJsonAsync("/api/v1/customers",
                                                                                         addCustomer);
                  });

            "Then the response contains a correctly formatted location header."
               .x(stepContext =>
                  {
                      var expectedUriBase = "http://localhost/api/v1/customers/";

                      httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.Created);
                      httpResponseMessage.Headers.Location.AbsoluteUri.ShouldStartWith(expectedUriBase);
                      string idSegment = httpResponseMessage.Headers.Location.Segments.Last();
                      Guid.TryParse(idSegment, out _).ShouldBeTrue($"The final segment of the URI could not be parsed to a GUID. This should be the customerId. The value was {idSegment}.");
                      var expectedAbsoluteUrl = $"{expectedUriBase}{idSegment}";
                      httpResponseMessage.Headers.Location.AbsoluteUri.ShouldBe(expectedAbsoluteUrl, StringCompareShould.IgnoreCase);
                  });
        }


        [Scenario]
        public void CustomerWithValidInputsReturnsFullyPopulatedCustomerObject(AddCustomer addCustomer, HttpResponseMessage httpResponseMessage)
        {
            "If the customers properties are populated correctly"
               .x(stepContext =>
                  {
                      addCustomer = new AddCustomer
                                    {
                                        FirstName = "TestFirstName",
                                        LastName = "TestLastName"
                                    };
                  });

            "When the Customer is posted to the Customers Controller"
               .x(async stepContext =>
                  {
                      httpResponseMessage = await _webApplicationFactory.CreateClient()
                                                                        .PostAsJsonAsync("/api/v1/customers",
                                                                                         addCustomer);
                  });

            "Then the response contains a correctly populated customer."
               .x(async stepContext =>
                  {
                      httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.Created);
                      var customer = await httpResponseMessage.Content.ReadFromJsonAsync<Customer>();
                      customer.ShouldNotBeNull();
                      customer.Id.ShouldNotBe(Guid.Empty);
                      customer.FirstName.ShouldBe(addCustomer.FirstName);
                      customer.LastName.ShouldBe(addCustomer.LastName);
                  });
        }
    }
}