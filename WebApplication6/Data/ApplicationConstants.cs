namespace WebApplication6.Data
{
    public static class ApplicationConstants
    {
        public const int MinUsernameLength = 3;
        public const int MinPhoneLength = 10;
        public const int MinUserAge = 18;

        public static class UserSettingKeys
        {
            public const string Theme = "theme";
            public const string Language = "language";
            public const string Notifications = "notifications";
            public const string Newsletter = "newsletter";
            public const string PrivacyLevel = "privacy_level";
        }

        public static class UserSettingValues
        {
            public const string ThemeLight = "light";
            public const string ThemeDark = "dark";
            public const string LanguageEn = "en";
            public const string LanguageRu = "ru";
            public const string NotificationsOn = "true";
            public const string NotificationsOff = "false";
            public const string NewsletterOn = "true";
            public const string NewsletterOff = "false";
            public const string PrivacyLevelHigh = "high";
            public const string PrivacyLevelMedium = "medium";
            public const string PrivacyLevelLow = "low";
        }

        public static class ValidationMessages
        {
            public const string UsernameInvalid = "Имя пользователя некорректно. Должно быть не менее {0} символов.";
            public const string EmailInvalid = "Email некорректен.";
            public const string PhoneInvalid = "Номер телефона некорректен. Должен быть не менее {0} цифр.";
            public const string UserTooYoung = "Пользователь должен быть старше {0} лет.";
            public const string UserAlreadyExists = "Пользователь с таким именем пользователя или email уже существует.";
        }

        public static class TemplateTypes
        {
            public const string Welcome = "Welcome";
            public const string Verification = "Verification";
            public const string Promotion = "Promotion";
        }
    }
}

