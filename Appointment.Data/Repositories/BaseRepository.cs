using Appointment.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Appointment.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class, IDeletableEntity
    {
        protected readonly AppointmentDbContext Context;
        private readonly IDbSet<T> set;

        public BaseRepository(AppointmentDbContext context)
        {
            this.Context = context;
            this.set = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set.AsQueryable();
        }

        public IQueryable<T> AllActive()
        {
            return this.set.AsQueryable().Where(s => !s.IsDeleted);
        }

        public virtual T GetById(object id)
        {
            return this.set.Find(id);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions)
        {
            return this.All().Where(conditions);
        }

        public void Add(T entity)
        {
            var entry = this.AttachIfDetached(entity);
            entry.State = EntityState.Added;
        }

        public void Update(T entity)
        {
            var entry = this.AttachIfDetached(entity);
            entry.State = EntityState.Modified;
        }

        public void Deactivate(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;
        }

        public void Deactivate(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedOn = DateTime.Now;
            }
        }

        public void Detach(T entity)
        {
            var entry = this.Context.Entry(entity);
            entry.State = EntityState.Detached;
        }

        public void Delete(T entity)
        {
            if (entity != null)
            {
                this.AttachIfDetached(entity);
                this.set.Remove(entity);
            }

            this.SaveChanges();
        }

        public virtual int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        private DbEntityEntry AttachIfDetached(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            return entry;
        }
    }
}
