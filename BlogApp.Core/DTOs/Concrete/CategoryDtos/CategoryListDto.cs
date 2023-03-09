﻿using BlogApp.Core.DTOs.Abstract;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CategoryListDto : IDto
    {
        public IList<Category> Categories { get; set; }
    }
}
