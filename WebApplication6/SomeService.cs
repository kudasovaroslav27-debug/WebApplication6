using Amazon.Runtime.Internal.Util;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using WebApplication6.SomeInterface;

namespace WebApplication6
{

    public class SomeService
    {
        private string _connectionString;
        private Logger _logger;
        private EmailService _emailService;
        private SMSService _smsService;

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


        public class SMSService : ISMSService
        {
            private readonly ILogger<SMSService> _logger;
            public SMSService(ILogger<SMSService> logger)
            {
                _logger = logger;
            }

            public async Task SendSmsAsync(string to, string message)
            {
                _logger.LogInformation($"Sending SMS to {to}. Message: {message}");
                await Task.Delay(100); // Имитация асинхронной работы
            }
        }
    }








