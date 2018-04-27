using Autofac;
using Autofac.Integration.Mvc;
using Blogbaster.Core;
using System.Reflection;
using System.Web.Mvc;

namespace Blogbaster
{
    public class AutoFacConfig
    {
        public static void RegisteModules()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule(new CoreModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}