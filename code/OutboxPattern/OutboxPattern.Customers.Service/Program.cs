using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OutboxPattern.Customers.Service
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                      .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                      .Build()
                      .RunAsync();
        }
    }
}