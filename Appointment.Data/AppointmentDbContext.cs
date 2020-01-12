using Appointment.Data.Migrations;
using Appointment.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Appointment.Data
{
    public class AppointmentDbContext : IdentityDbContext<User>, IAppointmentDbContext
    {
        public AppointmentDbContext() : base("DbPath", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppointmentDbContext, Configuration>());
        }

        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Partner> Partners { get; set; }
        public IDbSet<AppointmentType> AppointmentTypes { get; set; }
        public IDbSet<AppointmentInfo> AppointmentInfos { get; set; }

        public static AppointmentDbContext Create()
        {
            return new AppointmentDbContext();
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
