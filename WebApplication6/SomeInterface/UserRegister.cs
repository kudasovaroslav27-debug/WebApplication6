using WebApplication6.Dto;
using WebApplication6.Entities;

namespace WebApplication6.SomeInterface
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterUserDto dto);
    }
}
