using System;
using System.Collections.Generic;
using Appointment.Data.Filters;
using Appointment.Data.Proxies;
using Appointment.Data.Repositories;
using Appointment.Models;
using Appointment.Web.Infrastructure.Exceptions;
using Appointment.Web.Services;

namespace Appointment.Web.Areas.Appointments.Services
{
    public class AppointmentInfoService: BaseService
    {
        public AppointmentModel BuildModel()
        {
            return new AppointmentModel
            {
                Employees = this.RepoFactory.Get<EmployeeRepository>().GetActive(),
                Partners = this.RepoFactory.Get<PartnersRepository>().GetActive(),
                Types = this.RepoFactory.Get<AppointmentTypeRepository>().GetActive()
            };
        }

        public void Save(AppointmentInfoProxy proxy)
        {
            if (proxy.EmployeeId == Guid.Empty)
            {
                throw new UserException("Изберете служител");
            }

            if (proxy.PartnerId == Guid.Empty)
            {
                throw new UserException("Изберете партньор");
                
            }

            if (proxy.TypeId == Guid.Empty)
            {
                throw new UserException("Изберете тип");
            }

            if (proxy.Date == DateTime.MinValue)
            {
                throw new UserException("Изберете дата");
            }

            try
            {
                var repo = this.RepoFactory.Get<AppointmentInfoRepository>();
                var appointmentInfo = new AppointmentInfo
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = proxy.EmployeeId,
                    PartnerId = proxy.PartnerId,
                    TypeId = proxy.TypeId,
                    Date = proxy.Date
                };

                repo.Add(appointmentInfo);
                repo.SaveChanges();
            }
            catch (Exception)
            {
                throw new UserException("Грешка при записването на ангажиментът"); ;
            }
        }

        public IEnumerable<AppointmentInfoProxy> Get(AppointmentInfoFilter filter)
        {
            return this.RepoFactory.Get<AppointmentInfoRepository>().Get(filter);
        }
    }
}