using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.IdentityEntities;

namespace velora.services.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(Person user);

    }
}
