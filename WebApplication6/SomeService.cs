using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace WebApplication6
{
    /// <summary>
    /// Очень плохой сервис, please refactor me...
    /// Исправь его, и оставь комментарии, какие принципы и где нарушены.
    /// Дополнительно я тебе не рассказал, но есть такое понятие как magic numbers. magic string, тут они есть, узнай что это и исправь их
    /// Создавай дополнительные классы, чтобы это всё разбить на несколько, у доп классов не обязана быть реализиция,
    /// Напиши документацию на всё, используй комментарии подобного рода, summary
    /// просто создай их и вычлени с этого сервиса туда логику
    /// Понимаю, что ты не совсем знаком с ADO.NET, ознакомься со всеми непонятными методами оттуда, что они примерно делают
    /// Если что непонятно, обращайся к гптшке, но не проси его полностью сразу рефачить  код
    /// <remarks>
    /// Этот код не обязательно должен работать, просто тестовый пример
    /// </remarks>
    /// </summary>
    public class UserService
    {
        private string _connectionString;
        private Logger _logger;
        private EmailService _emailService;
        private SMSService _smsService;

        public UserService()
        {
            _connectionString = "Server=localhost;Database=myapp;User=admin;Password=123456;";
            _logger = new Logger();
            _emailService = new EmailService();
            _smsService = new SMSService();
        }

        public void ProcessUserRegistration(string username, string email, string phone, DateTime birthday, bool sendEmail, bool sendSMS, string templateType)
        {
            if (string.IsNullOrEmpty(username) || username.Length < 3)
            {
                throw new Exception("Username is invalid");
            }

            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new Exception("Email is invalid");
            }

            if (string.IsNullOrEmpty(phone) || phone.Length < 10)
            {
                throw new Exception("Phone is invalid");
            }

            var age = DateTime.Now.Year - birthday.Year;
            if (age < 18)
            {
                throw new Exception("User must be at least 18 years old");
            }

            // Работа с базой данных, точно ли тут нужно использовать ADO.NET? Может быть на ef core перейдём?
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @username OR Email = @email", connection);
                checkCmd.Parameters.AddWithValue("@username", username);
                checkCmd.Parameters.AddWithValue("@email", email);
                var exists = (int)checkCmd.ExecuteScalar() > 0;

                if (exists)
                {
                    throw new Exception("User already exists");
                }

                var insertCmd = new SqlCommand("INSERT INTO Users (Username, Email, Phone, Birthday, CreatedDate) VALUES (@username, @email, @phone, @birthday, @created)", connection);
                insertCmd.Parameters.AddWithValue("@username", username);
                insertCmd.Parameters.AddWithValue("@email", email);
                insertCmd.Parameters.AddWithValue("@phone", phone);
                insertCmd.Parameters.AddWithValue("@birthday", birthday);
                insertCmd.Parameters.AddWithValue("@created", DateTime.Now);
                insertCmd.ExecuteNonQuery();

                _logger.Write($"User {username} registered at {DateTime.Now}"); // Записываем лог (почитай что такое лог, логгер, зачем он нужен)

                // Создаем 5 дефолтных настроек для пользователя
                var settings1 = new SqlCommand("INSERT INTO UserSettings (Username, SettingKey, SettingValue) VALUES (@username, 'theme', 'light')", connection);
                settings1.Parameters.AddWithValue("@username", username);
                settings1.ExecuteNonQuery();

                var settings2 = new SqlCommand("INSERT INTO UserSettings (Username, SettingKey, SettingValue) VALUES (@username, 'language', 'en')", connection);
                settings2.Parameters.AddWithValue("@username", username);
                settings2.ExecuteNonQuery();

                var settings3 = new SqlCommand("INSERT INTO UserSettings (Username, SettingKey, SettingValue) VALUES (@username, 'notifications', 'true')", connection);
                settings3.Parameters.AddWithValue("@username", username);
                settings3.ExecuteNonQuery();

                var settings4 = new SqlCommand("INSERT INTO UserSettings (Username, SettingKey, SettingValue) VALUES (@username, 'timezone', 'UTC')", connection);
                settings4.Parameters.AddWithValue("@username", username);
                settings4.ExecuteNonQuery();

                var settings5 = new SqlCommand("INSERT INTO UserSettings (Username, SettingKey, SettingValue) VALUES (@username, 'currency', 'USD')", connection);
                settings5.Parameters.AddWithValue("@username", username);
                settings5.ExecuteNonQuery();

                // Создаем задачи для пользователя
                var task1 = new SqlCommand("INSERT INTO UserTasks (Username, TaskName, Priority) VALUES (@username, 'Complete profile', 1)", connection);
                task1.Parameters.AddWithValue("@username", username);
                task1.ExecuteNonQuery();

                var task2 = new SqlCommand("INSERT INTO UserTasks (Username, TaskName, Priority) VALUES (@username, 'Verify email', 2)", connection);
                task2.Parameters.AddWithValue("@username", username);
                task2.ExecuteNonQuery();

                var task3 = new SqlCommand("INSERT INTO UserTasks (Username, TaskName, Priority) VALUES (@username, 'Add photo', 3)", connection);
                task3.Parameters.AddWithValue("@username", username);
                task3.ExecuteNonQuery();

                if (sendEmail == true)
                {
                    if (templateType == "welcome")
                    {
                        _emailService.SendEmail(email, "Welcome!", "Welcome to our service!");
                        _emailService.SendEmail("support@company.com", "New user", $"New user {username} joined");
                        _emailService.SendEmail("admin@company.com", "New registration", $"User {username} registered");
                    }
                    else if (templateType == "premium")
                    {
                        _emailService.SendEmail(email, "Premium Welcome", "Welcome to premium service!");
                        _emailService.SendEmail("support@company.com", "New premium user", $"New premium user {username} joined");
                        _emailService.SendEmail("admin@company.com", "New premium registration", $"Premium user {username} registered");
                    }
                    else
                    {
                        _emailService.SendEmail(email, "Welcome", "Hello!");
                        _emailService.SendEmail("support@company.com", "New user", $"New user {username} joined");
                        _emailService.SendEmail("admin@company.com", "New registration", $"User {username} registered");
                    }
                }

                _logger.Write($"User {username} settings created");
                _logger.Write($"User {username} tasks created");
                _logger.Write($"User {username} notifications sent");

                if (sendSMS)
                {
                    _smsService.SendSMS(phone, "Welcome to our service!");
                }

                if (age > 65)
                {
                    var discountCmd = new SqlCommand("INSERT INTO UserDiscounts (Username, Discount) VALUES (@username, @discount)", connection);
                    discountCmd.Parameters.AddWithValue("@username", username);
                    discountCmd.Parameters.AddWithValue("@discount", 10);
                    discountCmd.ExecuteNonQuery();

                    var pensionSetting1 = new SqlCommand("INSERT INTO UserSettings (Username, SettingKey, SettingValue) VALUES (@username, 'senior_discount', 'true')", connection);
                    pensionSetting1.Parameters.AddWithValue("@username", username);
                    pensionSetting1.ExecuteNonQuery();

                    var pensionSetting2 = new SqlCommand("INSERT INTO UserSettings (Username, SettingKey, SettingValue) VALUES (@username, 'large_font', 'true')", connection);
                    pensionSetting2.Parameters.AddWithValue("@username", username);
                    pensionSetting2.ExecuteNonQuery();
                }

                var profileCmd = new SqlCommand("INSERT INTO UserProfiles (Username, Level, Points) VALUES (@username, @level, @points)", connection);
                profileCmd.Parameters.AddWithValue("@username", username);
                profileCmd.Parameters.AddWithValue("@level", 1);
                profileCmd.Parameters.AddWithValue("@points", 0);
                profileCmd.ExecuteNonQuery();

                var bonus1 = new SqlCommand("INSERT INTO UserBonuses (Username, Points, Reason) VALUES (@username, 1, 'Registration bonus')", connection);
                bonus1.Parameters.AddWithValue("@username", username);
                bonus1.ExecuteNonQuery();

                var bonus2 = new SqlCommand("INSERT INTO UserBonuses (Username, Points, Reason) VALUES (@username, 1, 'Welcome points')", connection);
                bonus2.Parameters.AddWithValue("@username", username);
                bonus2.ExecuteNonQuery();

                var bonus3 = new SqlCommand("INSERT INTO UserBonuses (Username, Points, Reason) VALUES (@username, 1, 'First login')", connection);
                bonus3.Parameters.AddWithValue("@username", username);
                bonus3.ExecuteNonQuery();
            }

            if (sendSMS)
            {
                _smsService.SendSMS(phone, "Welcome to our service!");
                _smsService.SendSMS(phone, "Your account is ready!");
                _smsService.SendSMS(phone, "Thank you for joining!");
            }
        }

        public void UpdateUserProfile(string username, string newEmail, string newPhone)
        {
            if (string.IsNullOrEmpty(newEmail) || !newEmail.Contains("@"))
            {
                throw new Exception("Email is invalid");
            }

            if (string.IsNullOrEmpty(newPhone) || newPhone.Length < 10)
            {
                throw new Exception("Phone is invalid");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("UPDATE Users SET Email = @email, Phone = @phone WHERE Username = @username", connection);
                cmd.Parameters.AddWithValue("@email", newEmail);
                cmd.Parameters.AddWithValue("@phone", newPhone);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();

                _logger.Write($"User {username} profile updated at {DateTime.Now}");
            }
        }
    }

    public class EmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            // Отправка email, реализация не обязательна
        }
    }

    public class SMSService
    {
        public void SendSMS(string phone, string message)
        {
            // Отправка SMS, реализация не обязательна
        }
    }

    public class Logger
    {
        public void Write(string message)
        {
            // Логирование, реализация не обязательна
        }
    }
}
