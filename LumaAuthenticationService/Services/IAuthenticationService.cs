using LumaAuthenticationService.Models;

namespace LumaAuthenticationService.Services
{
    public interface IAuthenticationService
    {
        string GenerateToken(User user);
        void RegisterUser(User user);
        void ConfirmEmail(string userId);
        string Login(UserLogin loginUser);
    }
}
