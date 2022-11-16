using LumaEmailService.Models;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace LumaEmailService.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response> SendEmail(Email emailMessage)
        {
            string apiKey = _configuration["SendGridConfiguration:SENDGRID_API_KEY"];
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["SendGridConfiguration:From"], _configuration["SendGridConfiguration:Name"]),
                Subject = emailMessage.Subject,
                PlainTextContent = emailMessage.TextContent,
                HtmlContent = emailMessage.HtmlContent
            };
            msg.AddTo(new EmailAddress(emailMessage.Recipient, emailMessage.RecipientUserName));

            if (emailMessage.SendAtUTC != 0)
                msg.SendAt = emailMessage.SendAtUTC;

            Response response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            return response;
        }
    }
}
