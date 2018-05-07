using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogbaster.Core.Data.Entities;
using Blogbaster.Core.Data.Enums;
using Blogbaster.Core.Services.Abstract;
using Blogbaster.Core.Services.Interfaces;

namespace Blogbaster.Core.Services
{
    public class PostService : BaseService<Post>, IPostService
    {
        public PostService(ApplicationDbContext context)
            : base(context) { }

        public IEnumerable<Post> GetPaginated(int pageIndex, int pageSize, string userId = null)
        {
            var res = Context.Posts
                .Where(a => a.Status == Status.Published)
                .OrderByDescending(a => a.DatePublished).AsQueryable()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                res = res.Where(p => p.ApplicationUserId == userId).ToList();
            }
            return res;
        }

        public int PublishedPostsCount(string userId = null)
        {
            int res;
            res = !string.IsNullOrWhiteSpace(userId)
                ? Context.Posts.Count(p => p.Status == Status.Published && p.ApplicationUserId == userId)
                : Context.Posts.Count(p => p.Status == Status.Published);

            return res;
        }

        public async Task ChangeStatus(Post post)
        {
            if (post.Status == Status.Created)
            {
                if (!post.WasPublished)
                {
                    post.WasPublished = true;
                }

                post.Status = Status.Published;
                post.DatePublished = DateTime.Now;
            }
            else
            {
                post.Status = Status.Created;
            }

            await Context.SaveChangesAsync();
        }
    }
}
