using System.Linq;
using System.Security.Claims;

namespace ProjectPlaguemangler.Extensions
{
    public static class IdentityExtensions
    {
        public static string Username(this ClaimsPrincipal self)
        {
            return self.Identity.Name;
        }

        public static int? Id(this ClaimsPrincipal self)
        {
            var claim = self.Claims.FirstOrDefault(c => c.Type == "UserIdentifier");

            if (claim == null) return null;

            if (int.TryParse(claim.Value, out int id))
            {
                return id;
            }

            return null;
        }
    }
}
