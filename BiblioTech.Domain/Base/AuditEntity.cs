

namespace BiblioTech.Domain.Base
{
    public abstract class AuditEntity<TType> : BaseEntity<TType>
    {
        public AuditEntity()
        {
            this.CreationDate = DateTime.Now;
        }
        public DateTime CreationDate { get; set; }
        public int CreationUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? ModifyUser { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedUser { get; set; }
        public bool? isDeleted { get; set; }
    }
}
