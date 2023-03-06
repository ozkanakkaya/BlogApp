using AutoMapper;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;

namespace BlogApp.Business.Services
{
    public class RoleService : Service<AppRole>, IRoleService
    {
        public RoleService(IGenericRepository<AppRole> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }


    }
}
