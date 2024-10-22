using BiblioTech.API.Models.Libros;
using BiblioTech.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Interfaces;
using Persistence.Models;

namespace BiblioTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ILibrosRepository librosRepository;

        public LibrosController(ILibrosRepository librosRepository)
        {
            this.librosRepository = librosRepository;
        }

        // GET: api/<LibrosController>
        [HttpGet("GetLibro")]
        public async Task<IActionResult> GetAll()
        {
            DataResults<List<LibrosModel>> result = new DataResults<List<LibrosModel>>();
            result = await librosRepository.GetAll();

            if (!result.Sucess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET api/<LibrosController>/byAutor
        [HttpGet("GetLibroByAutor")]
        public async Task<IActionResult> GetByAutor(string autor)
        {
            var result = await librosRepository.GetLibrosByAuthor(autor);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron libros para el autor especificado." });
            }

            return Ok(result);
        }

        // POST api/<LibrosController>
        [HttpPost("CreateLibro")]
        public async Task<IActionResult> Post([FromBody] LibrosCreateDTO librosCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newLibro = new BiblioTech.Domain.Entities.Libros
            {
                Tittle = librosCreateDTO.Tittle,
                Autor = librosCreateDTO.Autor,
                Genero = librosCreateDTO.Genero,
                ISBN = librosCreateDTO.ISBN,
                PublicationDate = librosCreateDTO.PublicationDate,
                ExpireDate = librosCreateDTO.ExpireDate,
                Status = librosCreateDTO.Status,
                CreationUser = librosCreateDTO.CreationUser,
                CreationDate = DateTime.UtcNow
            };

            bool result = await librosRepository.Create(newLibro);

            if (!result)
            {
                return BadRequest(new { Message = "Error al crear el libro." });
            }

            return Ok(new { Message = "Libro creado con éxito." });
        }

        // PUT api/<LibrosController>/5
        [HttpPut("UpdateLibro/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LibrosUpdateDTO librosUpdateDTO)
        {
            var libroExistente = await librosRepository.GetByCondition(libro => libro.Id == id && libro.isDeleted != true);
            if (libroExistente == null)
            {
                return NotFound(new { Message = "El libro no fue encontrado." });
            }

            libroExistente.Tittle = librosUpdateDTO.Tittle;
            libroExistente.Autor = librosUpdateDTO.Autor;
            libroExistente.Genero = librosUpdateDTO.Genero;
            libroExistente.ISBN = librosUpdateDTO.ISBN;
            libroExistente.PublicationDate = librosUpdateDTO.PublicationDate;
            libroExistente.ExpireDate = librosUpdateDTO.ExpireDate;
            libroExistente.Status = librosUpdateDTO.Status;
            libroExistente.ModifyUser = librosUpdateDTO.ModifyUser;
            libroExistente.ModifyDate = DateTime.UtcNow;

            bool result = await librosRepository.Update(libroExistente);

            if (!result)
            {
                return BadRequest(new { Message = "Error al actualizar el libro." });
            }

            return Ok(new { Message = "Libro actualizado con éxito." });
        }

        // DELETE api/<LibrosController>/5
        [HttpDelete("RemoveLibro/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await librosRepository.Remove(id);

            if (!result)
            {
                return NotFound(new { Message = "El libro no fue encontrado." });
            }

            return NoContent();
        }
    }
}
