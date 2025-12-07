using WebApplication6.Entities;

namespace WebApplication6.SomeInterface
{
    /// <summary>
    /// Интерфейс для сервиса управления настройками пользователя.
    /// </summary>
    public interface IUserSettingService
    {
        Task<ICollection<UserSetting>> CreateDefaultSettingsForUserAsync(int id);
    }
}