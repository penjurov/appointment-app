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
    public class AppointmentTypeRepository : BaseRepository<AppointmentType>
    {
        [InjectionConstructor]
        public AppointmentTypeRepository(AppointmentDbContext context)
            : base(context)
        {
        }

        public IEnumerable<AppointmentTypeProxy> Get(AdminFilter filter)
        {
            var result = this.All()
                .Where(x => !x.IsDeleted)
                .Where(filter.KeyWord, h => h.Name.Contains(filter.KeyWord))
                .OrderBy(h => h.Id);

            filter.Count = result.Count();

            return this.GetProxy(result.OrderByFilter(filter).PageByFilter(filter));
        }

        public IEnumerable<AppointmentTypeProxy> GetActive()
        {
            var result = this.All()
                .Where(x => !x.IsDeleted)
                .OrderBy(h => h.Name)
                .ToList();

            return this.GetProxy(result);
        }


        private IEnumerable<AppointmentTypeProxy> GetProxy(List<AppointmentType> result)
        {
            return result.Select(p => new AppointmentTypeProxy
            {
                Id = p.Id,
                Name = p.Name,
                Value = p.Value
            });
        }
    }
}
