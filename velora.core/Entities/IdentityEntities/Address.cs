using System.ComponentModel.DataAnnotations;
using velora.core.Entities.IdentityEntities;

namespace velora.core.Entities.IdentityEntities
{
    public class Address
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }    
        public string State { get; set; }
        public string ZipCode { get; set; }
        [Required]
        public string PersonId { get; set; }
        public Person Person { get; set; }


    }
}