using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        
        public string Username { get; set; }

        public string Email { get; set; }

        public bool Status { get; set; }

        public int BlogId { get; set; }

        public Blog Blog { get; set; }
    }
}
