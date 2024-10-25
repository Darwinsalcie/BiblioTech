

using BiblioTech.Domain.Base;

namespace BiblioTech.Domain.Entities
{
    public class Roles : AuditEntity<int>
    {
        public override int Id { get; set; }
        public string Role { get; set; }


    }
}
