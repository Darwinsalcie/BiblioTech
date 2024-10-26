using BiblioTech.API.Models.BaseDTO;

namespace BiblioTech.Api.Models.Usuarios
{
    public record UsuariosUpdateDTO : BaseUpdateEntityDTO
    {

        public string Name { get; set; }
        public string eMail { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }

    }
}
