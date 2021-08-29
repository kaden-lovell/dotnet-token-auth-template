using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Models;
using Server.Persistence;

namespace Server.Services {
    public class UserService {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _repository;
        public UserService(IHttpContextAccessor httpContextAccessor, IRepository<User> repository) {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<dynamic> GetActiveUserAsync() {
            var id = _httpContextAccessor.HttpContext.User.Identity.Id();
            var user = await _repository.GetUserById(id);

            if (user == null) {
                return null;
            }

            var result = new {
                id = id,
                username = user.Username,
                email = user.Email,
                role = user.Role
            };

            return result;
        }

        public async Task<dynamic> GetUserAsync(long id) {
            var user = await _repository.GetUserById(id);

            var result = new {
                id = user.Id,
                username = user.Username,
                email = user.Email,
                role = user.Role
            };

            return result;
        }

        public async Task<dynamic> AddUserAsync(dynamic model) {
            if (model.username == null || model.email == null) {
                return null;
            }

            var exists = await _repository.UserExistsByUsernameOrEmail((string)model.username, (string)model.email);

            if (exists) {
                var error = new {
                    errors = new {
                        userExists = true
                    }
                };

                return error;
            }

            var user = new User {
                Email = model.email,
                Username = model.username,
                Role = model.role,
                Password = model.password,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            await _repository.AddAsync(user);

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

        public async Task<dynamic> UpdateUserAsync(dynamic model) {
            var user = await _repository.GetUserById((long)model.id);

            if (user == null) {
                return null;
            }

            user.Email = model.email;
            user.Role = model.role;
            user.Username = model.username;

            await _repository.UpdateAsync(user);

            var updatedUser = await _repository.GetUserById(user.Id);

            var result = new {
                id = updatedUser.Id,
                email = updatedUser.Email,
                role = updatedUser.Role,
                username = updatedUser.Username
            };

            return result;
        }
    }
}