using LumaAuthenticationService.Data.Context;
using LumaAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace LumaAuthenticationService.Data
{
    public class AuthenticationRepository : IAuthenticationRepository, IDisposable
    {
        private readonly AuthenticationServiceContext _context;

        public AuthenticationRepository(AuthenticationServiceContext context)
        {
            _context = context;
        }

        public void RegisterUser(User user)
        {
            try
            {
                _context.Add(user);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ConfirmEmail(User user)
        {
            try
            {
                _context.LumaUsers.Update(user);
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User GetUserById(string id)
        {
            try
            {
                return _context.LumaUsers.AsNoTracking().FirstOrDefault(x => x.UserId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                return _context.LumaUsers.AsNoTracking().FirstOrDefault(x => x.UserName == username);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User GetUserByEmail(string email)
        {
            try
            {
                return _context.LumaUsers.AsNoTracking().FirstOrDefault(x => x.Email == email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
