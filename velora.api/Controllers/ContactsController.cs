using Microsoft.AspNetCore.Mvc;
using velora.services.Services.ContactsService.Dto;
using velora.services.Services.ContactsService;
using Microsoft.AspNetCore.Authorization;

namespace velora.api.Controllers
{
    //[Authorize]
    public class ContactsController : APIBaseController
    {
        private readonly IContactsService _contactsService;

        public ContactsController(IContactsService contactsService)
        {
            _contactsService = contactsService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitMessage([FromBody] ContactsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _contactsService.SubmitMessageAsync(dto);
                return Ok(new { message = "Your message has been received. We'll get back to you shortly." });
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}

