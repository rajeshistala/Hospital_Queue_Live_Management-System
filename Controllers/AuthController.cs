using HQLM_Backend.DTOs.Auth;
using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using HQLM_Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HQLM_Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher<Users> _passwordHasher;

        public AuthController(
            IUserRepository userRepository,
            JwtService jwtService,
            IPasswordHasher<Users> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            var roles = user.UserRoles
                .Where(ur => ur.Roles != null)
                .Select(ur => ur.Roles.RoleName)
                .Distinct()
                .ToList();

            var token = _jwtService.GenerateToken(user);

            return Ok(new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Roles = roles
            });
        }
    }
}