using Appointment.Common;
using Appointment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Appointment.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppointmentDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppointmentDbContext context)
        {
            this.SeedRoles(context);
            this.SeedUsers(context);
        }

        private void SeedUsers(AppointmentDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var store = new UserStore<User>(context);
            var manager = new UserManager<User>(store);

            var administrator = new User { UserName = GlobalConstants.AdministratorRoleName };
            manager.Create(administrator, GlobalConstants.InitialPassword);
            manager.AddToRole(administrator.Id, GlobalConstants.AdministratorRoleName);

            context.SaveChanges();
        }

        private void SeedRoles(AppointmentDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.Add(new IdentityRole { Name = GlobalConstants.AdministratorRoleName });
            context.SaveChanges();
        }
    }
}
