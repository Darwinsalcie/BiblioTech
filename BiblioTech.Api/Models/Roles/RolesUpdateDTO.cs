using BiblioTech.API.Models.BaseDTO;

namespace BiblioTech.Api.Models.Roles
{
    public record RolesUpdateDTO : BaseUpdateEntityDTO
    {
        public string Role { get; set; }
    }
}
