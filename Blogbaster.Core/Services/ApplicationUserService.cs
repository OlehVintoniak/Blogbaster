using Blogbaster.Core.Data.Entities;
using Blogbaster.Core.Services.Abstract;
using Blogbaster.Core.Services.Interfaces;

namespace Blogbaster.Core.Services
{
    public class ApplicationUserService : BaseService<ApplicationUser>, IApplicationUserService
    {
        public ApplicationUserService(ApplicationDbContext context)
            : base(context) { }
    }
}
