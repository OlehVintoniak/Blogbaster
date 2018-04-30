using Blogbaster.Core.Data.Entities;
using Blogbaster.Core.Services.Abstract;
using Blogbaster.Core.Services.Interfaces;

namespace Blogbaster.Core.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(ApplicationDbContext context)
            : base(context) { }
    }
}
