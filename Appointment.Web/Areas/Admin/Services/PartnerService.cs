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
    public class PartnerService : BaseService
    {
        public List<PartnerProxy> Get(AdminFilter filter)
        {
            return this.RepoFactory.Get<PartnersRepository>().Get(filter).ToList();
        }

        public Guid Upsert(PartnerProxy proxy)
        {
            try
            {
                var repo = this.RepoFactory.Get<PartnersRepository>();
                Partner partner;

                if (proxy.Id.HasValue)
                {
                    partner = repo.GetById(proxy.Id);
                }
                else
                {
                    partner = new Partner();
                    partner.Id = Guid.NewGuid();
                    repo.Add(partner);
                }

                partner.Name = proxy.Name;

                repo.SaveChanges();

                return partner.Id;
            }
            catch (Exception)
            {
                throw new UserException("Грешка при записването на партньора");
            }
        }

        public void Remove(Guid id)
        {
            var repo = this.RepoFactory.Get<PartnersRepository>();
            var partner = repo.GetById(id);

            if (partner != null)
            {
                partner.IsDeleted = true;
                partner.DeletedOn = DateTime.Now;
                repo.SaveChanges();
            }
            else
            {
                throw new UserException("Грешка при изтриването на партньора");
            }
        }
    }
}