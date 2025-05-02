using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.IdentityEntities;

namespace velora.repository.Interfaces.IdentityInterfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetByIdAsync(string userId);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByEmailAsync(string email);
        Task UpdateAsync(Person person);
    }
}
