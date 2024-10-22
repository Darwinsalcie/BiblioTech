﻿

using BiblioTech.Domain.Base;

namespace Persistence.Models
{
    public class LibrosModel: AuditEntity<int>
    {
        public override int Id { get; set; }

        public string Tittle { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public string ISBN { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Status { get; set; }

    }
}