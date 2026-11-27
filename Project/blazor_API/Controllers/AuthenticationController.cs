using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recommendation_engine;

namespace Recommendations_engine
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _context; // Empty. But every class still needs to access this.

        public AuthenticationController(AppDbContext context)
        {
            _context = context; // Fill the empty value with the one that is filled. Context is appdbcontext class that is now created with ctor.
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] AppUser insert_user)
        {
            if (insert_user == null)
            {
                return BadRequest("User data is required.");
            }

            // Add user to database
            _context.AppUsers.Add(insert_user);

            // Save changes
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User added successfully",
                user = insert_user
            });
        }

        [HttpPost("GetUser")]
        public async Task<ActionResult<AppUser>> GetUser([FromBody] string UserId)
        {
            if (string.IsNullOrWhiteSpace(UserId))
            {
                return BadRequest("UserId is required.");
            }

            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.UserId == UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }
    }
}
