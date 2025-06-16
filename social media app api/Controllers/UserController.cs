
using global::social_media_app_api.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_media_app_api.DTO;
using social_media_app_api.JWTHelper;
using social_media_app_api.Models;
using social_media_app_api.Services;


namespace social_media_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JWTGenerator _jwtGenerator;

        public UserController(IUserService userService, JWTGenerator jwtGenerator)
        {
            _userService = userService;
            _jwtGenerator = jwtGenerator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

   
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, RegisterDto user)
        {
            var updated = await _userService.UpdateUserAsync(id, user);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateAsync(login.Username, login.Password);

            if (user == null)
                return Unauthorized(new { message = "Fel användarnamn eller lösenord." });

            var token = _jwtGenerator.JWTTokenGenereator(user);
            return Ok(new
            {
                token,
                userId = user.UserId,
                username = user.Username,
                name = user.Name
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = await _userService.RegisterUserAsync(dto);

            var token = _jwtGenerator.JWTTokenGenereator(user);
            return Ok(new { Token = token });
        }
    }
}


