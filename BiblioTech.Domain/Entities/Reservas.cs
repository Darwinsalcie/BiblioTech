﻿

using BiblioTech.Domain.Base;

namespace BiblioTech.Domain.Entities
{
    public class Reservas : AuditEntity<int>
    {
        public override int Id { get; set ; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string Status { get; set; }
    }
}
