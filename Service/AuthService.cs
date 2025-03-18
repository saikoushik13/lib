using AutoMapper;
using Database.Entities;
using DatabaseOperations.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.DTO;
using Service.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserDatabaseOperations _userDbOperations;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(
            IUserDatabaseOperations userDbOperations,
            IMapper mapper,
            IConfiguration config)
        {
            _userDbOperations = userDbOperations;
            _mapper = mapper;
            _config = config;
        }

        public async Task<UserResponseDto> RegisterAsync(UserRegisterDto userDto)
        {
            var existingUser = await _userDbOperations.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
                throw new Exception("Email already registered.");

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = Constants.RoleEnum.StandardUser
            };

            await _userDbOperations.AddAsync(user);
            await _userDbOperations.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userDbOperations.GetByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
