using Microsoft.Extensions.Options;
using Store.Repository.Interfaces;
using System.Net.Mail;
using System.Net;
using velora.core.Entities;
using velora.services.Services.ContactsService.Dto;
using Microsoft.Extensions.Logging;
using AutoMapper;
using velora.services.Helper;

namespace velora.services.Services.ContactsService
{

    public class ContactsService : IContactsService
    {
        private readonly IUnitWork _unitWork;
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<ContactsService> _logger;
        private readonly IMapper _mapper;

        public ContactsService(IUnitWork unitWork, IOptions<EmailSettings> emailSettings, ILogger<ContactsService> logger, IMapper mapper)
        {
            _unitWork = unitWork;
            _emailSettings = emailSettings.Value;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SubmitMessageAsync(ContactsDto dto)
        {
            try
            {
                
                var message = _mapper.Map<Contacts>(dto);

                await _unitWork.Repository<Contacts, int>().AddAsync(message);
                await _unitWork.CompleteAsync();


                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.From),
                    Subject = "New Contact Us Message",
                    Body = $"From: {dto.FirstName} {dto.LastName}\nEmail: {dto.Email}\nPhone: {dto.PhoneNumber}\nMessage:\n{dto.Message}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(_emailSettings.From); 

                using var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.From, _emailSettings.Password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting the contact message.");
                throw new Exception("An error occurred while processing your message. Please try again later.");
            }
        }
    }
}
