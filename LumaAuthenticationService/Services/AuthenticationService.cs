using LumaAuthenticationService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using LumaAuthenticationService.Data;

namespace LumaAuthenticationService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationService(IConfiguration configuration, IAuthenticationRepository authenticationRepository)
        {
            _configuration = configuration;
            _authenticationRepository = authenticationRepository;
        }

        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] secret = Encoding.ASCII.GetBytes(_configuration["Secret"]);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(User user)
        {
            try
            {
                ValidateUsername(user.UserName);

                if(GetUserByUsername(user.UserName.Trim()) != null)
                    throw new ArgumentException("Este username já está sendo utilizado!");

                if (GetUserByEmail(user.Email.Trim()) != null)
                    throw new ArgumentException("Este e-mail já está sendo utilizado!");

                ValidatePasswordLength(user.Password.Trim());
                user.Password = Encrypt(user.Password.Trim());
                _authenticationRepository.RegisterUser(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ConfirmEmail(string userId)
        {
            try
            {
                User user = GetUserById(userId.Trim());

                if (user == null)
                    throw new ArgumentException("Identificador inválido!");

                user.UpdateEmailConfirmed();
                _authenticationRepository.ConfirmEmail(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Login(UserLogin loginUser)
        {
            try
            {
                User user = GetUserByUsername(loginUser.UserName.Trim());

                if (user == null) 
                    throw new ArgumentException("Usuário não cadastrado!");

                if (user.EmailConfirmed == 0)
                    throw new ArgumentException("Confirmação de e-mail pendente!");

                ValidatePassword(loginUser.Password.Trim(), user.Password);
                return GenerateToken(user);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User GetUserById(string userId)
        {
            try
            {
                return _authenticationRepository.GetUserById(userId);
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
                return _authenticationRepository.GetUserByUsername(username);
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
                return _authenticationRepository.GetUserByEmail(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string Encrypt(string data)
        {
            string hash = String.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));

                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }

            return hash;
        }

        private static void ValidatePassword(string password, string encryptedPassword)
        {
            if (Encrypt(password) != encryptedPassword)
                throw new ArgumentException("Senha incorreta!");
        }

        private static void ValidatePasswordLength(string password)
        {
            if (password.Length < 8)
                throw new ArgumentException("A senha deve conter ao menos oito caracteres!");
        }

        private static void ValidateUsername(string username)
        {
            if (username.Length < 5)
                throw new ArgumentException("O username deve conter ao menos 5 caracteres!");
        }
    }
}
