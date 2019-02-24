using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pm.Data.Entity;
using Pm.Core.Interfaces;
using Pm.Data.Interfaces;
using Pm.Common.Exceptions;

namespace Pm.Core
{
    public class UserService
    {
        private readonly IRepository<User> userRepo;
        private readonly IPasswordDerivationService passwordService;

        public UserService(IRepository<User> userRepo, IPasswordDerivationService passwordService)
        {
            this.userRepo = userRepo;
            this.passwordService = passwordService;
        }

        public async Task SeedRootUser(string username, string password, string displayName)
        {
            var existing = await userRepo.Query()
                .SingleOrDefaultAsync(u => u.Username == username);
            
            if (existing != null) return;

            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordService.CreateHash(password),
                DisplayName = displayName
            };

            await userRepo.Create(newUser);
        }

        public async Task<User> GetVerifiedUser(string username, string password)
        {
            var user = await userRepo.Query()
                .SingleOrDefaultAsync(u => u.Username == username);

            if (user == null) throw new PmNotFoundException("Username not found");

            if (!passwordService.VerifyHash(user.PasswordHash, password))
            {
                throw new PmNotFoundException("Invalid password for given username");
            }

            return user;
        } 
    }
}
