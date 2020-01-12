using System;
using System.Data;
using System.Data.Common;
using Appointment.Data;
using Appointment.Data.RepoFactory;
using Appointment.Models;
using Appointment.Web.Areas.Admin.Services;
using Appointment.Web.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;

namespace Appointment.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IRepoFactory, RepoFactory>();
            container.RegisterType<IAppointmentDbContext, AppointmentDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<User>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<User>, UserStore<User>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());

            container.RegisterType<EmployeeService>();
        }
    }
}
