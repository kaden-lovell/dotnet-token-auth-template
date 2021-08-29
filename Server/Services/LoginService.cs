using System.IO;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Server.Models;
using Server.Persistence;

namespace Server.Services {
    public class LoginService {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _repository;
        public LoginService(IHttpContextAccessor httpContextAccessor, IRepository<User> repository) {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        public async Task<dynamic> LoginAsync(dynamic model) {
            var user = await _repository.GetUserByEmailAsync((string)model.email);

            if (user == null) {
                var error = new {
                    errors = new {
                        userNotFound = true
                    }
                };

                return error;
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, $"{user.Id}"),
                new Claim("https://localhost:4200/claims/username", user.Username),
                new Claim("https://localhost:4200/claims/email", user.Email),
                new Claim("https://localhost:4200/claims/id", user.Id.ToString()),
                new Claim("https://localhost:4200/claims/role", user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var authProperties = new AuthenticationProperties {
                IsPersistent = true,
                RedirectUri = "https://localhost:4200/login"
            };

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await _httpContextAccessor.HttpContext.SignInAsync("Cookies", claimsPrincipal);

            await CreateCookieFile(claimsPrincipal);

            var result = new {
                success = true,
                user = new {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    role = user.Role
                }
            };

            return result;
        }

        public async Task LogoutAsync() {
            DeleteCookieFile(_httpContextAccessor.HttpContext.User.Identity.Name);
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

        private async Task CreateCookieFile(ClaimsPrincipal claimsPrincipal) {
            var keysDirectoryPath = @"C:\Users\kaden\Desktop\local_cookies";
            if (!Directory.Exists(keysDirectoryPath)) {
                Directory.CreateDirectory(keysDirectoryPath);
            };

            using StreamWriter file = new(@$"{keysDirectoryPath}\{claimsPrincipal.Identity.Name}.txt");

            await file.WriteLineAsync("---claims---");
            foreach (var claim in claimsPrincipal.Claims) {
                await file.WriteLineAsync($"{claim.Type}: {claim.Value ?? "null"}");
            }

            await file.WriteLineAsync("---identity info---");
            await file.WriteLineAsync($"Name: {claimsPrincipal.Identity.Name ?? "null"}");
            await file.WriteLineAsync($"IsAuthenticated: {claimsPrincipal.Identity.IsAuthenticated.ToString() ?? "null"}");
            await file.WriteLineAsync($"AuthenticationType: {claimsPrincipal.Identity.AuthenticationType ?? "null"}");
            await file.WriteLineAsync($"Role: {claimsPrincipal.Identity.Role() ?? "null"}");
        }

        private void DeleteCookieFile(string identityName) {
            var keysDirectoryPath = @"C:\Users\kaden\Desktop\local_cookies";
            File.Delete(@$"{keysDirectoryPath}\{identityName}.txt");
        }
    }
}