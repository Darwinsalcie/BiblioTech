using BiblioTech.API.Models.BaseDTO;

namespace BiblioTech.Api.Models.Roles
{
    public record RolesCreateDTO : BaseSaveEntityDTO
    {
        public string Role { get; set; }
    }
}
