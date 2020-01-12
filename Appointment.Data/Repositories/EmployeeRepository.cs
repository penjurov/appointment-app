using Appointment.Common.Helpers;
using Appointment.Data.Filters;
using Appointment.Data.Helpers;
using Appointment.Data.Proxies;
using Appointment.Models;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace Appointment.Data.Repositories
{
    public class EmployeeRepository: BaseRepository<Employee>
    {
        [InjectionConstructor]
        public EmployeeRepository(AppointmentDbContext context)
            : base(context)
        {
        }

        public IEnumerable<EmployeeProxy> Get(AdminFilter filter)
        {
            var result = this.All()
                .Where(x => !x.IsDeleted)
                .Where(filter.KeyWord, h => h.Name.Contains(filter.KeyWord))
                .OrderBy(h => h.Id);

            filter.Count = result.Count();

            return this.GetProxy(result.OrderByFilter(filter).PageByFilter(filter));
        }

        public IEnumerable<EmployeeProxy> GetActive()
        {
            var result = this.All()
                .Where(x => !x.IsDeleted)
                .OrderBy(h => h.Name)
                .ToList();

            return this.GetProxy(result);
        }

        private IEnumerable<EmployeeProxy> GetProxy(List<Employee> result)
        {
            return result.Select(p => new EmployeeProxy
            {
                Id = p.Id,
                Name = p.Name
            });
        }
    }
}
