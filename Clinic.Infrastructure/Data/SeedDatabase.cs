using Clinic.Core.Constant;
using Clinic.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Data
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
            var defaultBand = dbContext.Users
                .FirstOrDefault(c => c.Id.Equals(SystemConstant.SeedConst.USER_ID));

            if (defaultBand is null)
            {
                defaultBand = new AppUser
                {
                    Id = SystemConstant.SeedConst.USER_ID,
                    UserName = SystemConstant.SeedConst.USER_NAME,
                };
                dbContext.Add(defaultBand);
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
