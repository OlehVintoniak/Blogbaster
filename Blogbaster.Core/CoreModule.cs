using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace Blogbaster.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ApplicationDbContext)).As(typeof(ApplicationDbContext)).InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            base.Load(builder);
        }
    }
}
