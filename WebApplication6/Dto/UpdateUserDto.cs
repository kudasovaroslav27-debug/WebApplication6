using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Dto
{
    public record UpdateUserDto
    {   
        public string UserName { get; set; }

        public string Email { get; set; }

        public int Id { get; set; }
    }
}
