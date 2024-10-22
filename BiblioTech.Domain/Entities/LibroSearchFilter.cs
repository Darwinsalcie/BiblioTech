// BiblioTech.Domain/Models/LibroSearchFilter.cs
namespace BiblioTech.Domain.Models
{
    public class LibroSearchFilter
    {
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Genero { get; set; }
        public int? AñoDesde { get; set; }
        public int? AñoHasta { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? OrderBy { get; set; }
        public bool Ascending { get; set; } = true;
    }
}