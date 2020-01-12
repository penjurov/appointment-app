using Appointment.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Appointment.Data
{
    public interface IAppointmentDbContext
    {
        IDbSet<Employee> Employees { get; set; }
        IDbSet<Partner> Partners { get; set; }
        IDbSet<AppointmentType> AppointmentTypes { get; set; }
        IDbSet<AppointmentInfo> AppointmentInfos { get; set; }

        IDbSet<User> Users { get; set; }
        IDbSet<IdentityRole> Roles { get; set; }

        IDbSet<T> Set<T>()
            where T : class;

        DbEntityEntry<T> Entry<T>(T entity)
            where T : class;

        int SaveChanges();

        void Dispose();
    }
}
