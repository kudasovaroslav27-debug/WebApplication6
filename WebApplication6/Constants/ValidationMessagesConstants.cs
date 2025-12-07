namespace WebApplication6.Constants
{
    public class ValidationMessages
    {
        // Сообщения валидации
        public const string UsernameInvalid = "Имя пользователя некорректно. Должно быть не менее {0} символов.";
        public const string EmailInvalid = "Email некорректен.";
        public const string PhoneInvalid = "Номер телефона некорректен. Должен быть не менее {0} цифр.";
        public const string UserTooYoung = "Пользователь должен быть старше {0} лет.";
        public const string UserAlreadyExists = "Пользователь с таким именем пользователя или email уже существует.";
    }
}
