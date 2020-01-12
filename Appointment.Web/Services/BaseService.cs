using Appointment.Data.RepoFactory;
using Microsoft.Practices.Unity;

namespace Appointment.Web.Services
{
    public abstract class BaseService
    {
        [Dependency]
        public IRepoFactory RepoFactory { get; set; }
    }
}