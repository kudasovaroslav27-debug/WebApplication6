using Microsoft.EntityFrameworkCore;
using WebApplication6.Constants;
using WebApplication6.Data;
using WebApplication6.Dto;
using WebApplication6.Entities;
using WebApplication6.SomeInterface;

namespace WebApplication6
{
    public record UserServiced : IUserService
    {
        private readonly IApplicationDbContext _context;
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
            _context = context;
            _logger = logger;
            _emailService = emailService;
            _smsService = smsService;
            _userSettingService = userSettingService;
        }

        public async Task<User> RegisterUserAsync(RegisterUserDto dto)
        {
            try
            {
                // 1. Валидация возраста пользователя
                var age = DateTime.Now.Year - dto.Birthday.Year;
                if (age < ValidationConstants.MinUserAge)
                {
                    _logger.LogWarning("Попытка регистрации пользователя младше {MinUserAge} лет. Email: {Email}", ValidationConstants.MinUserAge, dto.Email);
                    throw new ArgumentException(string.Format(ValidationMessages.UserTooYoung, ValidationConstants.MinUserAge));
                }

                // Проверка  пользователя по Username или Email
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == dto.Username || u.Email == dto.Email);

                if (existingUser != null)
                {
                    string conflictField = existingUser.Username == dto.Username ? "имя пользователя" : "email";
                    _logger.LogWarning("Попытка регистрации существующего пользователя. Конфликт по {ConflictField}. Username: {Username}, Email: {Email}", conflictField, dto.Username, dto.Email);
                    throw new ArgumentException($"Пользователь с таким {conflictField} уже существует.");
                }

                //Создание новой сущности пользователя
                var newUser = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Birthday = dto.Birthday,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(); // Сохраняем пользователя, сгенерированный ID

                _logger.LogInformation("Пользователь {Username} (ID: {UserId}) успешно добавлен в базу данных.", newUser.Username, newUser.Id);


                newUser.UserSettings = await _userSettingService.CreateDefaultSettingsForUserAsync(newUser.Id);

                //Отправка email и SMS (асинхронно и неблокирующе)
                var notificationTasks = new List<Task>();

                if (dto.SendEmail)
                {
                    var emailSubject = $"Добро пожаловать, {newUser.Username}!";
                    var emailBody = GetEmailTemplate(dto.TemplateType, newUser);

                    notificationTasks.Add(_emailService.SendEmailAsync(newUser.Email, emailSubject, emailBody)
                        .ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                _logger.LogError(task.Exception, "Ошибка при отправке приветственного письма для пользователя {Email}", newUser.Email);
                            }
                            else
                            {
                                _logger.LogInformation("Отправлено приветственное письмо для пользователя {Email}", newUser.Email);
                            }
                        }));
                }

                if (dto.SendSMS)
                {
                    var smsMessage = GetSmsTemplate(dto.TemplateType, newUser);

                    notificationTasks.Add(_smsService.SendSmsAsync(newUser.Phone, smsMessage)
                        .ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                _logger.LogError(task.Exception, "Ошибка при отправке приветственного SMS для пользователя {Phone}", newUser.Phone);
                            }
                            else
                            {
                                _logger.LogInformation("Отправлено приветственное SMS для пользователя {Phone}", newUser.Phone);
                            }
                        }));
                }

                _logger.LogInformation("Регистрация пользователя {Username} (ID: {UserId}) успешно завершена.", newUser.Username, newUser.Id);

                return newUser;
            }
            catch (ArgumentException ex) //Для ошибок валидации
            {
                _logger.LogWarning(ex, "Ошибка бизнес-логики при регистрации пользователя: {Message}", ex.Message);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при регистрации пользователя {Username}.", dto.Username);
                throw new Exception("Произошла ошибка при сохранении пользователя в базу данных.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Непредвиденная ошибка при регистрации пользователя {Username}.", dto.Username);
                throw new Exception("Произошла непредвиденная ошибка во время регистрации пользователя.", ex);
            }
        }

        private string GetSmsTemplate(string templateType, User user)
        {
            return templateType switch
            {
                TemplateTypes.Welcome => $"Привет, {user.Username}! Добро пожаловать!",
                _ => $"Привет, {user.Username}! Ваше уведомление."
            };
        }
        private string GetEmailTemplate(string templateType, User user)
        {
            return templateType switch
            {
                TemplateTypes.Welcome => $"Здравствуйте, {user.Username}! Добро пожаловать в наш сервис!",
                TemplateTypes.Verification => $"Здравствуйте, {user.Username}! Ваш код подтверждения: 12345.", 
                TemplateTypes.Promotion => $"Здравствуйте, {user.Username}! У нас новые акции!",
                _ => $"Здравствуйте, {user.Username}! Ваше уведомление."
            };
        }
    }
}











