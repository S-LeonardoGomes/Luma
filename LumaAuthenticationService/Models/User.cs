using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace LumaAuthenticationService.Models
{
    public class User
    {
        public string UserId { get; private set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string Password { get; set; }
        
        [EmailAddress(ErrorMessage = "O campo de e-mail não está no formato esperado")]
        public string Email { get; set; }
        public int EmailConfirmed { get; private set; } = 0;
        public string Role { get; private set; } = "Common";

        public void UpdateEmailConfirmed() => EmailConfirmed = 1;
    }
}
