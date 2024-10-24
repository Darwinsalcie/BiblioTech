using BiblioTech.API.Models.BaseDTO;

namespace BiblioTech.API.Models.Notificaciones
{
    public record class NotificacionesCreateDTO : BaseSaveEntityDTO
    {

        public string Type { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public DateTime FechaEnvio { get; set; }
    }
}
