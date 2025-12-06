using WebApplication6.SomeInterface;

namespace WebApplication6.UserService
{
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
