using Blogbaster.Core.Data.Entities;
using Blogbaster.Core.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogbaster.Core.Services.Interfaces
{
    public interface IPostService : IBaseService<Post>
    {
        IEnumerable<Post> GetPaginated(int pageIndex, int pageSize, string userId = null);
        int PublishedPostsCount(string userId = null);
        Task ChangeStatus(Post post);
    }
}