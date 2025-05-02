using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using velora.core.Entities.IdentityEntities;

namespace velora.core.Entities.OrderEntities
{

    public class ShippingAddress
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
  
    }
}