using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Appointment.Web.Startup))]
namespace Appointment.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
