using BiblioTech.API.Models.BaseDTO;

namespace BiblioTech.API.Models.Libros
{
    public record class LibrosCreateDTO : BaseSaveEntityDTO
    {
        public string Tittle { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public string ISBN { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Status { get; set; }
    }
}
