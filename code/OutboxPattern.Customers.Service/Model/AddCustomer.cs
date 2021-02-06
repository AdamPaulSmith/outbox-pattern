using System.ComponentModel.DataAnnotations;

namespace OutboxPattern.Customers.Service.Model
{
    public class AddCustomer
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}