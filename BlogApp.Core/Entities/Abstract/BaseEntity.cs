namespace BlogApp.Core.Entities.Abstract
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime CreatedDate { get; set; }

        public virtual DateTime UpdatedDate { get; set; }

        public virtual string CreatedByUsername { get; set; }

        public virtual string UpdatedByUsername { get; set; }

        public virtual bool IsDeleted { get; set; } = false;

        public virtual bool IsActive { get; set; } = true;
    }
}
