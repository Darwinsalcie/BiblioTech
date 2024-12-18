﻿using BiblioTech.Domain.Base;

namespace BiblioTech.Domain.Entities
{
    public class Notificaciones : AuditEntity<int>
    {
        public override int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public DateTime FechaEnvio { get; set; }

    }
}
