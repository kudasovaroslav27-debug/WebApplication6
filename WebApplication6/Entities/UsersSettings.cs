using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication6.Entities
{
        public class UserSetting
        {
            [Key]
            public int Id { get; set; }

            [ForeignKey("Users")]
            public int UserId { get; set; }

            public User User { get; set; }
        }
    }

