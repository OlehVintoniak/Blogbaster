using Autofac;
using Blogbaster.Core;

namespace Blogbaster.Modules
{
    public class EfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());

            //builder.RegisterType(typeof(ApplicationDbContext)).As(typeof(ApplicationDbContext)).InstancePerLifetimeScope();
        }
    }
}