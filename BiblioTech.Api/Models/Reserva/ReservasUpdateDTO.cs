using BiblioTech.API.Models.BaseDTO;

namespace BiblioTech.Api.Models.Reserva
{
    public record ReservasUpdateDTO : BaseUpdateEntityDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string Status { get; set; }
    }
}
