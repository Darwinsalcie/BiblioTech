using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models
{
    public class ReservasModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string Status { get; set; }
        //public int? CreationUser { get; set; }
        //public DateTime? CreationDate { get; set; }
        //public int? ModifyUser { get; set; }
        //public DateTime? ModifyDate { get; set; }
        //public int? DeleteUser { get; set; }
        //public DateTime? DeletedDate { get; set; }
        //public bool isDeleted { get; set; } = false; // Valor predeterminado
    }
}
