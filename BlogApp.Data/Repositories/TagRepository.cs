using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.DTOs.Concrete.TagDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }

        public void AddBlogWithTags(BlogCreateDto createDto, List<TagListDto> tagList)
        {
            //return _context.TagBlogs.AddAsync(tagList);
        }
    }
}
