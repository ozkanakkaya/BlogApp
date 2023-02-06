using BlogApp.Core.DTOs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs.Concrete.CommentDtos
{
    public class CommentDto : IDto
    {
        public string Content { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int BlogId { get; set; }
    }
}
