using Clinic.Core.Constant;
using Clinic.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Clinic.Core.Helper;
using Microsoft.AspNetCore.Identity;

namespace Clinic.Data.Data
{
    public static class SeedDatabase
    {
        public static void MigrateAndSeedDb(this IApplicationBuilder app, bool development = false)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
            {
                //your development/live logic here eg:
                context.Migrate();
                if (development)
                    context.Seed();
            }
        }

        internal static ApplicationDbContext AddOrUpdateSeedData(this ApplicationDbContext dbContext)
        {
            // Create seed users in system
            var defaultUser = dbContext.Users
                .FirstOrDefault(c => c.Id.Equals(SystemConstant.SeedConst.USER_ID));
            if (defaultUser is null)
            {
                defaultUser = new AppUser
                {
                    Id = SystemConstant.SeedConst.USER_ID,
                    UserName = SystemConstant.SeedConst.USER_NAME,
                    NormalizedUserName = SystemConstant.SeedConst.USER_NAME.ToUpper(),
                    Role = UserRoles.Admin,
                    IsActive = true
                };
                var hasher = new PasswordHasher<AppUser>();
                defaultUser.PasswordHash = hasher.HashPassword(defaultUser, SystemConstant.SeedConst.USER_PASSWORD);
                dbContext.Add(defaultUser);
            }

            // Create seed roles in system
            var defaultRoles = dbContext.Roles.Any();
            if (!defaultRoles)
            {
                foreach (var role in SystemConstant.SeedConst.ROLES_IDS)
                {
                    dbContext.Add(new RoleUser
                    {
                        Name = role.Key.ToString(),
                        NormalizedName = role.Key.ToString().ToUpper(),
                        Id = role.Value,
                    });
                }
            }

            // Create seed user role
            var defaultUserRole = dbContext.UserRoles.Any(x => 
                x.UserId.Equals(SystemConstant.SeedConst.USER_ID) && x.RoleId.Equals(SystemConstant.SeedConst.ROLES_IDS[UserRoles.Admin]));
            if (!defaultUserRole)
            {
                dbContext.Add(new IdentityUserRole<string>
                {
                    RoleId = SystemConstant.SeedConst.ROLES_IDS[UserRoles.Admin],
                    UserId = SystemConstant.SeedConst.USER_ID
                });
            }
            return dbContext;
        }

        private static void Migrate(this ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
        }
        private static void Seed(this ApplicationDbContext context)
        {
            context.AddOrUpdateSeedData();
            context.SaveChanges();
        }
    }
}
