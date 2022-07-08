using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class AppRole : BaseEntity, IEntity
    {
        public string Definition { get; set; }

        public ICollection<AppUserRole> AppUserRoles { get; set; }
    }
}
