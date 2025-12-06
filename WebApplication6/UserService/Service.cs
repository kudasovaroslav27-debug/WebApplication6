using Microsoft.Extensions.Logging;
using WebApplication6.Dto;
using WebApplication6.Entities;
using WebApplication6.SomeInterface;

namespace WebApplication6.UserService
{
    
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;

        public UserService(
            ApplicationDbContext context,
            ILogger<UserService> logger, // <-- ILogger инжектируется в конструктор
            IEmailService emailService,
            ISMSService smsService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        }

        Task<User> IUserService.RegisterUserAsync(RegisterUserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
