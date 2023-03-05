using BlogApp.Core.DTOs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CommentUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        //public int BlogId { get; set; }
    }
}
