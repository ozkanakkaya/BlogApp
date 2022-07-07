using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<TagBlog> TagBlogs { get; set; }
    }
}
