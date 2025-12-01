using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Dto;
using WebApplication6.Entities;
using WebApplication6.SomeInterface;

namespace WebApplication6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto userUpdateDto)
        {

            var userToUpdate = await _context.Users.FindAsync(id);
            userToUpdate.Username = userUpdateDto.UserName;
            userToUpdate.Email = userUpdateDto.Email;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}