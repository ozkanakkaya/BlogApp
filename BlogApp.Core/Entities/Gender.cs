using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Entities
{
    public class Gender : BaseEntity
    {
        public string Definition { get; set; }

        public ICollection<AppUser> AppUsers { get; set; }
    }
}
