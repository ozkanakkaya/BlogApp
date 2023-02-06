using BlogApp.Core.DTOs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class BlogSearchModel : BaseDto, IDto
    {
        public List<BlogListDto> BlogListDto { get; set; }
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
    }
}
