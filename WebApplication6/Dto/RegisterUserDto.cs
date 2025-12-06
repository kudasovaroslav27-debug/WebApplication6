using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Dto
{

    public class RegisterUserDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime Birthday { get; set; }
    }
}
