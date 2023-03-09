namespace BlogApp.Core.Entities.Abstract
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual DateTime UpdatedDate { get; set; } = DateTime.Now;

        public virtual string CreatedByUsername { get; set; } = "Admin";

        public virtual string UpdatedByUsername { get; set; } = "Admin";

        public virtual bool IsDeleted { get; set; } = false;

        public virtual bool IsActive { get; set; } = true;
    }
}
