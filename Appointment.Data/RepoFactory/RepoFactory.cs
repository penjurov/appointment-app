using System.Web.Mvc;

namespace Appointment.Data.RepoFactory
{
    public class RepoFactory : IRepoFactory
    {
        public T Get<T>() where T : class
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}