using HQLM_Backend.DTOs.Auth;
using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using HQLM_Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HQLM_Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserRepository userRepository,
            JwtService jwtService,
            IPasswordHasher<Users> passwordHasher,
            ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDTO request)
        {
            _logger.LogInformation("Login request started.");

            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning(
                    "Login failed because email or password was empty.");

                return BadRequest(new
                {
                    message = "Email and password are required."
                });
            }

            var user = await _userRepository.GetByEmailAsync(
                request.Email.Trim());

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning(
                    "Login failed. User not found or inactive.");

                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning(
                    "Login failed due to invalid credentials.");

                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var token = _jwtService.GenerateToken(user);

            var roles = user.UserRoles
                .Where(ur => ur.Roles != null)
                .Select(ur => ur.Roles.RoleName)
                .Distinct()
                .ToList();

            _logger.LogInformation(
                "Login successful. UserId: {UserId}",
                user.UserId);

            return Ok(new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Roles = roles,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }

        // POST: api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            RegisterRequestDTO request)
        {
            _logger.LogInformation("User registration started.");

            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning(
                    "Registration failed due to missing required fields.");

                return BadRequest(new
                {
                    message = "Username, email and password are required."
                });
            }

            var existingUser = await _userRepository
                .GetByEmailAsync(request.Email.Trim());

            if (existingUser != null)
            {
                _logger.LogWarning(
                    "Registration failed because email already exists.");

                return Conflict(new
                {
                    message = "Email already exists."
                });
            }

            var user = new Users
            {
                UserName = request.UserName.Trim(),
                Email = request.Email.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation(
                "User registered successfully. UserId: {UserId}",
                user.UserId);

            return Ok(new
            {
                message = "User registered successfully.",
                userId = user.UserId,
                email = user.Email
            });
        }
    }
}