using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using OutboxPattern.Customers.Service;

namespace OutboxPattern.Customers.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                              .ConfigureWebHostDefaults(webHostBuilder =>
                                                        {
                                                            webHostBuilder.UseStartup<Startup>()
                                                                          .UseTestServer();
                                                        });
            return builder;
        }
    }
}