

using BiblioTech.Domain.Base;

namespace BiblioTech.Domain.Entities
{
    public class Usuarios : AuditEntity<int>
    {
        public override int Id { get; set; }
        public  int RoleId { get; set; }
        public string Name { get; set; }
        public string eMail { get; set; }
        public string Password { get; set; }
        public string RoleName {  get; set; }
        

    }
}
