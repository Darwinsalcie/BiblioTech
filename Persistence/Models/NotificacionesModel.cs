namespace Persistence.Models
{
    public class NotificacionesModel
    {
        public  int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool isDeleted { get; set; } = false; // Valor predeterminado
    }
}
