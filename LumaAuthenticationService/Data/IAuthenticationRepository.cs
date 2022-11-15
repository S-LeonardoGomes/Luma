using LumaAuthenticationService.Models;

namespace LumaAuthenticationService.Data
{
    public interface IAuthenticationRepository
    {
        void RegisterUser(User user);
        void ConfirmEmail(User user);
        User GetUserById(string userId);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
    }
}
