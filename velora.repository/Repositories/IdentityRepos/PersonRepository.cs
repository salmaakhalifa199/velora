using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.IdentityEntities;
using velora.repository.Interfaces.IdentityInterfaces;

namespace velora.repository.Repositories.IdentityRepos
{
    public class PersonRepository : IPersonRepository
    {
        private readonly UserManager<Person> _userManager;

        public PersonRepository(UserManager<Person> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Person> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return _userManager.Users;
        }

        public async Task<Person> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task UpdateAsync(Person person)
        {
            await _userManager.UpdateAsync(person);
        }
    }
}
