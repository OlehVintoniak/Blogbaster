using Blogbaster.Core.Data.Entities;
using Blogbaster.Core.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogbaster.Core.Services.Interfaces
{
    public interface IPostService : IBaseService<Post>
    {
        IEnumerable<Post> GetPaginated(int pageIndex, int pageSize);
        int PublishedPostsCount();
        Task ChangeStatus(Post post);
    }
}