using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PrioritiZe.Web.Models.DomainModels;
using PrioritiZe.Web.Models.DTO;
using PrioritiZe.Web.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PrioritiZe.Web.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(string? Token, string? UserId, IdentityResult Result)> RegisterAsync(RegistrationDTO registrationDTO)
        {
            var user = new User
            {
                UserName = registrationDTO.Username,
                Email = registrationDTO.Email,
                FirstName = registrationDTO.FirstName,
                LastName = registrationDTO.LastName,
                JobTitle = registrationDTO.JobTitle,
                EmailConfirmed = true
            };
            if (registrationDTO.ProfilePictureFile == null)
            {
                user.ProfilePictureSrc = "avatar.png";
            }
            else
            {
                user.ProfilePictureSrc = await SaveImage(registrationDTO.ProfilePictureFile);
            }

            if (registrationDTO.Password == registrationDTO.ConfirmPassword)
            {
                var result = await _userManager.CreateAsync(user, registrationDTO.Password);

                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(user.UserName);
                    var token = GenerateJwtToken(user);
                    return (token, user.Id, result);
                }
                else
                {
                    return (null, null, result);
                }
            }
            else
            {
                return (null, null, IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." }));
            }
        }

        public async Task<(string? Token, string? UserId, IdentityResult Result)> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.Username);

            if (user == null)
            {
                return (null, null, IdentityResult.Failed(new IdentityError { Description = "Invalid Email." }));
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result)
            {
                return (null, null, IdentityResult.Failed(new IdentityError { Description = "Invalid Password." }));
            }

            var token = GenerateJwtToken(user);
            return (token, user.Id, IdentityResult.Success);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }
}
