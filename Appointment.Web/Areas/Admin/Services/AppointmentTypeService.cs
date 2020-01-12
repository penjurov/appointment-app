using Appointment.Data.Filters;
using Appointment.Data.Proxies;
using Appointment.Data.Repositories;
using Appointment.Models;
using Appointment.Web.Infrastructure.Exceptions;
using Appointment.Web.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Appointment.Web.Areas.Admin.Services
{
    public class AppointmentTypeService : BaseService
    {
        public List<AppointmentTypeProxy> Get(AdminFilter filter)
        {
            return this.RepoFactory.Get<AppointmentTypeRepository>().Get(filter).ToList();
        }

        public Guid Upsert(AppointmentTypeProxy proxy)
        {
            try
            {
                var repo = this.RepoFactory.Get<AppointmentTypeRepository>();
                AppointmentType appointmentType;

                if (proxy.Id.HasValue)
                {
                    appointmentType = repo.GetById(proxy.Id);
                }
                else
                {
                    appointmentType = new AppointmentType();
                    appointmentType.Id = Guid.NewGuid();
                    repo.Add(appointmentType);
                }

                appointmentType.Name = proxy.Name;
                appointmentType.Value = proxy.Value;

                repo.SaveChanges();

                return appointmentType.Id;
            }
            catch (Exception)
            {
                throw new UserException("Грешка при записването на видът ангажимент"); ;
            }
        }

        public void Remove(Guid id)
        {
            var repo = this.RepoFactory.Get<AppointmentTypeRepository>();
            var appointmentType = repo.GetById(id);

            if (appointmentType != null)
            {
                appointmentType.IsDeleted = true;
                appointmentType.DeletedOn = DateTime.Now;
                repo.SaveChanges();
            }
            else
            {
                throw new UserException("Грешка при изтриването на видът ангажимент");
            }
        }
    }
}