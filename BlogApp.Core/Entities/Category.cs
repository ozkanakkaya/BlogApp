﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Entities
{
    public class Category : BaseEntity
    {       
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
