using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Persistence {
    public static partial class RepositoryExtensions {
        // linq syntax: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
        public static async Task<User> GetUserById(this IRepository<User> repository, long id) {
            var result =
                await repository
                    .AsQueryable()
                    .OfType<User>()
                    .SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public static async Task<User> GetUserByEmailAsync(this IRepository<User> repository, string email) {
            var result =
                await repository
                    .AsQueryable()
                    .OfType<User>()
                    .SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            return result;
        }

        public static async Task<bool> UserExistsByUsernameOrEmail(this IRepository<User> repository, string username, string email) {
            var result =
                await repository
                    .AsQueryable()
                    .OfType<User>()
                    .AnyAsync(x => x.Email.ToLower() == email.ToLower());

            return result;
        }

        public static async Task<bool> UsersExist(this IRepository<User> repository) {
            var result =
                await repository
                    .AsQueryable()
                    .OfType<User>()
                    .AnyAsync();

            return result;
        }
    }
}