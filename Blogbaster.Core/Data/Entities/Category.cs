using System.Collections.Generic;
using System.ComponentModel;
using Blogbaster.Core.Data.Entities.Abstract;

namespace Blogbaster.Core.Data.Entities
{
    public class Category : Entity<int>
    {
        [DisplayName("Категорія")]
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public Category()
        {
            Posts = new List<Post>();
        }
    }
}
