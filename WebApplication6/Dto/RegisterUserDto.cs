using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Dto
{

    public record RegisterUserDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime Birthday { get; set; }

        public bool SendEmail { get; internal set; }

        public string TemplateType { get; internal set; }

        public bool SendSMS { get; internal set; }
    }
}
