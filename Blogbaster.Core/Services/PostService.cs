using Blogbaster.Core.Data.Entities;
using Blogbaster.Core.Services.Abstract;
using Blogbaster.Core.Services.Interfaces;

namespace Blogbaster.Core.Services
{
    public class PostService : BaseService<Article>, IPostService
    {
        public PostService(ApplicationDbContext context)
            : base(context) { }
    }
}
