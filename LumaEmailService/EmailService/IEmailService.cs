using LumaEmailService.Models;
using SendGrid;

namespace LumaEmailService.EmailService
{
    public interface IEmailService
    {
        Task<Response> SendEmail(Email emailMessage);
    }
}
