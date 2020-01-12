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
    public class PartnersRepository : BaseRepository<Partner>
    {
        [InjectionConstructor]
        public PartnersRepository(AppointmentDbContext context)
            : base(context)
        {
        }

        public IEnumerable<PartnerProxy> Get(AdminFilter filter)
        {
            var result = this.All()
                .Where(x => !x.IsDeleted)
                .Where(filter.KeyWord, h => h.Name.Contains(filter.KeyWord))
                .OrderBy(h => h.Id);

            filter.Count = result.Count();

            return this.GetProxy(result.OrderByFilter(filter).PageByFilter(filter));
        }

        public IEnumerable<PartnerProxy> GetActive()
        {
            var result = this.All()
                .Where(x => !x.IsDeleted)
                .OrderBy(h => h.Name)
                .ToList();

            return this.GetProxy(result);
        }

        private IEnumerable<PartnerProxy> GetProxy(List<Partner> result)
        {
            return result.Select(p => new PartnerProxy
            {
                Id = p.Id,
                Name = p.Name
            });
        }
    }
}
