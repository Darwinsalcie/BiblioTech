using BiblioTech.Api.Models.Reserva;
using BiblioTech.API.Models.Libros;
using BiblioTech.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Interfaces;
using Persistence.Models;
using Persistence.Repositories;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BiblioTech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {

        private readonly IReservasRepository reservasRepository;

        public ReservasController(IReservasRepository reservasRepository)
        {
            this.reservasRepository = reservasRepository;
        }



        // GET: api/<ReservasController>
        [HttpGet("GetReservas")]
        public async Task<IActionResult> GetAll()
        {
            DataResults<List<ReservasModel>> result = new DataResults<List<ReservasModel>>();
            result = await reservasRepository.GetAll();

            if (!result.Sucess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET api/<ReservasController>/5
        // GET api/<LibrosController>/byAutor
        [HttpGet("GetReservasById")]
        public async Task<IActionResult> GetReservasById(int Id)
        {
            var result = await reservasRepository.GetReservasById(Id);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron reservas para el Id especificado." });
            }

            return Ok(result);
        }

        // POST api/<ReservasController>
        [HttpPost("CreateReserva")]
        public async Task<IActionResult> Post([FromBody] ReservasCreateDTO reservasCreateDTO)
        {
            bool result = false;

            result = await this.reservasRepository.Create(new Domain.Entities.Reservas()
            {
                UserId = reservasCreateDTO.UserId,
                BookId = reservasCreateDTO.BookId,
                ReservationDate = reservasCreateDTO.ReservationDate,
                Status = reservasCreateDTO.Status,
                CreationUser = reservasCreateDTO.CreationUser,
                CreationDate = reservasCreateDTO.CreationDate,
                //isDeleted = librosCreateDTO.IsDeleted = false,
            });

            if (!result)
            {
                return BadRequest(new { Message = "Error al crear la reserva." });
            }

            return Ok(new { Message = "Libro creado con éxito." });
        }

        // PUT api/<ReservasController>/5
        [HttpPut("UpdateReserva/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ReservasUpdateDTO reservasUpdateDTO)
        {
            var reservaExistente = await reservasRepository.GetByCondition(reserv => reserv.Id == id && reserv.isDeleted != true);
            if (reservaExistente == null)
            {
                return NotFound(new { Message = "El libro no fue encontrado." });
            }

            reservaExistente.UserId = reservasUpdateDTO.UserId;
            reservaExistente.BookId = reservasUpdateDTO.BookId;
            reservaExistente.Status = reservasUpdateDTO.Status;
            reservaExistente.ReservationDate = reservasUpdateDTO.ReservationDate;

                //isDeleted = librosCreateDTO.IsDeleted = false,
            reservaExistente.ModifyUser = reservasUpdateDTO.ModifyUser;
            reservaExistente.ModifyDate = DateTime.UtcNow;

            bool result = await reservasRepository.Update(reservaExistente);

            if (!result)
            {
                return BadRequest(new { Message = "Error al actualizar el libro." });
            }

            return Ok(new { Message = "Libro actualizado con éxito." });
        }

        // DELETE api/<ReservasController>/5
        [HttpDelete("RemoverReserva/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await reservasRepository.Remove(id);

            if (!result)
            {
                return NotFound(new { Message = "la reserva no fue encontrada." });
            }

            return NoContent();
        }
    }
}
