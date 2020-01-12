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
    public class EmployeeService: BaseService
    {
        public List<EmployeeProxy> Get(AdminFilter filter)
        {
            return this.RepoFactory.Get<EmployeeRepository>().Get(filter).ToList();
        }

        public Guid Upsert(EmployeeProxy proxy)
        {
            try
            {
                var repo = this.RepoFactory.Get<EmployeeRepository>();
                Employee employee;

                if (proxy.Id.HasValue)
                {
                    employee = repo.GetById(proxy.Id);
                }
                else
                {
                    employee = new Employee();
                    employee.Id = Guid.NewGuid();
                    repo.Add(employee);
                }

                employee.Name = proxy.Name;

                repo.SaveChanges();

                return employee.Id;
            }
            catch (Exception)
            {
                throw new UserException("Грешка при записването на служителя");
            }
        }

        public void Remove(Guid id)
        {
            var repo = this.RepoFactory.Get<EmployeeRepository>();
            var employee = repo.GetById(id);

            if (employee != null)
            {
                employee.IsDeleted = true;
                employee.DeletedOn = DateTime.Now;
                repo.SaveChanges();
            }
            else
            {
                throw new UserException("Грешка при изтриването на служителя");
            }
        }
    }
}