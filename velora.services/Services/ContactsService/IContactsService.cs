using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.services.Services.ContactsService.Dto;

namespace velora.services.Services.ContactsService
{
    public interface IContactsService
    {
        Task SubmitMessageAsync(ContactsDto messageDto);
    }
}
