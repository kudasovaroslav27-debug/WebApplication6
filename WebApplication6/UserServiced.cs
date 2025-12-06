using Amazon.Runtime.Internal.Util;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication6.Data;
using WebApplication6.Dto;
using WebApplication6.Entities;
using WebApplication6.SomeInterface;

namespace WebApplication6
{
    public class UserServiced : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserServiced> _logger;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;
        private readonly IUserSettingService _userSettingService;

        public UserServiced(
            ApplicationDbContext context,
            ILogger<UserServiced> logger,
            IEmailService emailService,
            ISMSService smsService,
            IUserSettingService userSettingService) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
            _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));
        }
        public IUserSettingService UserSettingService => _userSettingService;

        public async Task<User> RegisterUserAsync(RegisterUserDto dto)
        {
            //  Валидация возраста пользователя
            var age = DateTime.Now.Year - dto.Birthday.Year;
            if (age < ApplicationConstants.MinUserAge)
            {
                _logger.LogWarning("Попытка регистрации пользователя младше {MinUserAge} лет. Email: {Email}", ApplicationConstants.MinUserAge, dto.Email);
                throw new ArgumentException(string.Format(ApplicationConstants.ValidationMessages.UserTooYoung, ApplicationConstants.MinUserAge));
            }

            // Проверка пользователя по Username или Email
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email);

            if (userExists)
            {
                _logger.LogWarning("Попытка регистрации существующего пользователя. Username: {Username}, Email: {Email}", dto.Username, dto.Email);
                throw new ArgumentException(ApplicationConstants.ValidationMessages.UserAlreadyExists);
            }

            // Создание новой сущности пользователя
            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Phone = dto.Phone,
                Birthday = dto.Birthday,
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(newUser);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Пользователь {Username} (ID: {UserId}) успешно добавлен в базу данных.", newUser.Username, newUser.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении нового пользователя {Username} в базу данных.", dto.Username);
                throw new Exception("Не удалось сохранить пользователя в базу данных.", ex);
            }

            // Создание дефолтных настроек с помощью UserSettingService
            newUser.UserSettings = await _userSettingService.CreateDefaultSettingsForUserAsync(newUser.Id);

            _logger.LogInformation("Пользователь {Username} (ID: {UserId}) успешно зарегистрирован.", newUser.Username, newUser.Id);

            //  Отправка email и SMS 
            if (dto.SendEmail)
            {
                var emailSubject = $"Добро пожаловать, {newUser.Username}!";
                var emailBody = GetEmailTemplate(dto.TemplateType, newUser);
                await _emailService.SendEmailAsync(newUser.Email, emailSubject, emailBody);
                _logger.LogInformation("Отправлено приветственное письмо для пользователя {Email}", newUser.Email);
            }

            if (dto.SendSMS)
            {
                var smsMessage = GetSmsTemplate(dto.TemplateType, newUser);
                await _smsService.SendSmsAsync(newUser.Phone, smsMessage);
                _logger.LogInformation("Отправлено приветственное SMS для пользователя {Phone}", newUser.Phone);
            }

            return newUser;
        }

        private string GetEmailTemplate(string templateType, User user)
        {
            return templateType switch
            {
                ApplicationConstants.TemplateTypes.Welcome => $"Здравствуйте, {user.Username}! Добро пожаловать в наш сервис!",
                _ => $"Здравствуйте, {user.Username}! Ваше уведомление."
            };
        }

        private string GetSmsTemplate(string templateType, User user)
        {
            return templateType switch
            {
                ApplicationConstants.TemplateTypes.Welcome => $"Привет, {user.Username}! Добро пожаловать!",
                _ => $"Привет, {user.Username}! Ваше уведомление."
            };
        }
    }
}











