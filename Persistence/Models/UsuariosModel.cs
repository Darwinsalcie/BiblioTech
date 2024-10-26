

namespace Persistence.Models
{
    public class UsuariosModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string eMail { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
