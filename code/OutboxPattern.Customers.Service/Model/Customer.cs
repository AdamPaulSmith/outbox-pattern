using System;
using System.ComponentModel.DataAnnotations;

namespace OutboxPattern.Customers.Service.Model
{
    public class Customer
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}