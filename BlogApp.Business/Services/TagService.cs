using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;

namespace BlogApp.Business.Services
{
    public class TagService : Service<Tag>, ITagService
    {
        public TagService(IGenericRepository<Tag> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
