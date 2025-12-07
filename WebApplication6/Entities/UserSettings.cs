using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Entities
{
    public class UserSetting
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")] 
        public int UserId { get; set; }

        public string SettingKey { get; set; } = string.Empty; 

        public string SettingValue { get; set; } = string.Empty; 

        public User User { get; set; } = null!; 
    }
}

