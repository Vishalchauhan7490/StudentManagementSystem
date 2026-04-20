using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StudentManagementSystem.Helpers;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelper _jwt;
        private readonly AuthSettings _authSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            JwtHelper jwt,
            IOptions<AuthSettings> authOptions,
            ILogger<AuthController> logger)
        {
            _jwt = jwt;
            _authSettings = authOptions.Value;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // ✅ Model validation
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login request");
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid request data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            // ✅ Check credentials
            if (model.Username == _authSettings.Username &&
                model.Password == _authSettings.Password)
            {
                var token = _jwt.GenerateToken(model.Username);

                _logger.LogInformation("User logged in successfully: {Username}", model.Username);

                return Ok(new
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token
                });
            }

            // ❌ Invalid login
            _logger.LogWarning("Invalid login attempt for user: {Username}", model.Username);

            return Unauthorized(new
            {
                Success = false,
                Message = "Invalid username or password"
            });
        }
    }
}