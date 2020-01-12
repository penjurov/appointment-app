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
    public class AppointmentInfoRepository : BaseRepository<AppointmentInfo>
    {
        [InjectionConstructor]
        public AppointmentInfoRepository(AppointmentDbContext context)
            : base(context)
        {
        }

        public IEnumerable<AppointmentInfoProxy> Get(AppointmentInfoFilter filter)
        {
            var result = this.All()
                    .Where(filter.EmployeeId.HasValue, x => x.EmployeeId == filter.EmployeeId)
                    .Where(filter.PartnerId.HasValue, x => x.PartnerId == filter.PartnerId)
                    .Where(filter.TypeId.HasValue, x => x.TypeId == filter.TypeId)
                    .Where(filter.StartDate.HasValue, x => x.Date >= filter.StartDate)
                    .Where(filter.EndDate.HasValue, x => x.Date <= filter.EndDate)
                    .Where(x => !x.IsDeleted)
                    .OrderBy(h => h.Id)
                    .Select(x => new AppointmentInfoProxy
                    {
                        EmployeeId = x.EmployeeId,
                        EmployeeName = x.Employee.Name,
                        PartnerId = x.PartnerId,
                        PartnerName = x.Partner.Name,
                        TypeId = x.TypeId,
                        TypeName = x.Type.Name,
                        TypeValue = x.Type.Value,
                        Date = x.Date
                    });

            filter.Count = result.Count();

            if (filter.Count > 0)
            {
                filter.TotalValue = result.Sum(x => x.TypeValue);
            }

            return result.OrderByFilter(filter).PageByFilter(filter);
        }
    }
}
