using Microsoft.AspNetCore.Identity;
using PrioritiZe.Web.Models.DTO;

namespace PrioritiZe.Web.Services.Interface
{
    public interface IAuthService
    {
        Task<(string? Token, string? UserId, IdentityResult Result)> RegisterAsync(RegistrationDTO registrationDTO);
        Task<(string? Token, string? UserId, IdentityResult Result)> LoginAsync(LoginDTO loginDTO);
        Task<string> SaveImage(IFormFile imageFile);
    }
}
