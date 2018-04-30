using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Blogbaster.Core.Data.Entities.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blogbaster.Core.Data.Entities
{
    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        public ApplicationUser()
        {
            Posts = new List<Post>();
        }

        public virtual ICollection<Post> Posts { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
