using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public class TagService : Service<Tag>, ITagService
    {
        public TagService(IGenericRepository<Tag> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
