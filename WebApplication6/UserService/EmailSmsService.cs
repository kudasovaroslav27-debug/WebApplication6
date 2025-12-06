using WebApplication6.SomeInterface;

namespace WebApplication6
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;


        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            _logger.LogInformation($"Sending email to {to} with subject '{subject}'. Body: {body}");
            await Task.Delay(100);
        }
    }
}

