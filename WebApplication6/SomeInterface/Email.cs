using WebApplication6.Dto;
using WebApplication6.Entities;

namespace WebApplication6.SomeInterface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
   
}
