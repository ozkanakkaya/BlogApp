using BlogApp.Core.DTOs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class PersonalBlogDto : IDto
    {
        public List<BlogListDto> Blogs { get; set; }
        public int TotalBlogCount { get; set; }
        public int TotalActiveBlogCount { get; set; }
        public int TotalInactiveBlogCount { get; set; }
        public int TotalBlogsViewedCount { get; set; }

    }
}
