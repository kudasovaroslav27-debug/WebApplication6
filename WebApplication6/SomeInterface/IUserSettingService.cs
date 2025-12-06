using WebApplication6.Entities;

namespace WebApplication6.SomeInterface
{
    public interface IUserSettingService
    {
        Task<ICollection<UserSetting>> CreateDefaultSettingsForUserAsync(int id);
    }
}