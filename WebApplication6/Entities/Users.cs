using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Entities
{
    public record User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime Birthday { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
    }
}
