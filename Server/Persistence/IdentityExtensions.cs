using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Server.Persistence {
    public static class IdentityExtensions {
        public static string EmailAddress(this IIdentity identity) {
            var result = identity.GetValue("https://localhost:4200/claims/emailaddress");

            return result;
        }

        public static string FirstName(this IIdentity identity) {
            var result = identity.GetValue("https://localhost:4200/claims/firstname");

            return result;
        }

        public static long Id(this IIdentity identity) {
            var value = identity.GetValue("https://localhost:4200/claims/id");

            var result =
              value == null
                ? -1
                : long.Parse(value);

            return result;
        }

        public static string LastName(this IIdentity identity) {
            var result = identity.GetValue("https://localhost:4200/claims/lastname");

            return result;
        }

        public static string Username(this IIdentity identity) {
            var value = identity.GetValue("https://localhost:4200/claims/username");
            var result = value ?? "system";

            return result;
        }

        public static string Role(this IIdentity identity) {
            var result = identity.GetValue("https://localhost:4200/claims/role");

            return result;
        }

        // utility
        public static void AttemptAddClaim(this IIdentity identity, string type, string value) {
            if (!(identity is ClaimsIdentity ci) || !ci.IsAuthenticated || string.IsNullOrWhiteSpace(value)) {
                return;
            }

            var claim = ci.Claims.SingleOrDefault(x => x.Type == type && x.Value == value);

            if (claim == null) {
                ci.AddClaim(new Claim(type, value));
            }
        }

        public static void AttemptRemoveClaim(this IIdentity identity, string type, string value = null) {
            if (!(identity is ClaimsIdentity ci) || !ci.IsAuthenticated) {
                return;
            }

            var claim =
              value == null
                ? ci.Claims.SingleOrDefault(x => x.Type == type)
                : ci.Claims.SingleOrDefault(x => x.Type == type && x.Value == value);

            if (claim != null) {
                ci.RemoveClaim(claim);
            }
        }

        private static string GetValue(this IIdentity identity, string type) {
            if (!(identity is ClaimsIdentity ci) || !ci.IsAuthenticated) {
                return null;
            }

            var claim = ci.Claims.SingleOrDefault(x => x.Type == type);
            var result = claim?.Value;

            return result;
        }
    }
}