using LumaAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace LumaAuthenticationService.Data.Context
{
    public class AuthenticationServiceContext : DbContext
    {
        public AuthenticationServiceContext(DbContextOptions<AuthenticationServiceContext> options) : base(options) { }
        
        public DbSet<User> LumaUsers { get; set; }
    }
}
