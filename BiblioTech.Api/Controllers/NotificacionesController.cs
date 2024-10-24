using BiblioTech.API.Models.Notificaciones;
using BiblioTech.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Interfaces;
using Persistence.Models;


namespace BiblioTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionesRepository notificacionesRepository;

        public NotificacionesController(INotificacionesRepository notificacionesRepository)
        {
            this.notificacionesRepository = notificacionesRepository;
        }

        // GET: api/<NotificacionesController>
        [HttpGet("GetNotificacion")]
        public async Task<IActionResult> GetAll()
        {
            DataResults<List<NotificacionesModel>> result = new DataResults<List<NotificacionesModel>>();
                
                result = await notificacionesRepository.GetAll();

            if (!result.Sucess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET api/<NotificacionesController>/byUsuarioDestino
        [HttpGet("GetNotificacionByUsuarioDestino")]
        public async Task<IActionResult> GetByUsuarioDestino(int UserId)
        {
            var result = await notificacionesRepository.GetNotificacionesByUsuarioDestino(UserId);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron libros para el autor especificado." });
            }


            return Ok(result);
        }
    
        // POST api/<NotificacionesController>
        [HttpPost("CreateNotificacion")]
        public async Task<IActionResult> Post([FromBody] NotificacionesCreateDTO notificacionesCreateDTO)
        {
            bool result = false;
            result = await this.notificacionesRepository.Create(new Domain.Entities.Notificaciones()
            {
                Type = notificacionesCreateDTO.Type,
                Message= notificacionesCreateDTO.Message,
                UserId = notificacionesCreateDTO.UserId,
                FechaEnvio = notificacionesCreateDTO.FechaEnvio,
                CreationUser = notificacionesCreateDTO.CreationUser,
                CreationDate = notificacionesCreateDTO.CreationDate,
            });

            if (!result)
            {
                return BadRequest(new { Message = "Error al crear la notificación." });
        }

            return Ok(new { Message = "Notificación creada con éxito." });
        }

        // PUT api/<NotificacionesController>/5
        [HttpPut("UpdateNotificacion/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NotificacionesUpdateDTO notificacionesUpdateDTO)
        {
            var notificacionExistente = await notificacionesRepository.GetByCondition(n => n.Id == id && n.isDeleted != true);
            if (notificacionExistente == null)
            {
                return NotFound(new { Message = "La notificación no fue encontrada." });
            }

            notificacionExistente.Type = notificacionesUpdateDTO.Type;
            notificacionExistente.Message = notificacionesUpdateDTO.Message;
            notificacionExistente.UserId = notificacionesUpdateDTO.UserId;
            notificacionExistente.FechaEnvio = notificacionesUpdateDTO.FechaEnvio;
            notificacionExistente.ModifyUser = notificacionesUpdateDTO.ModifyUser;
            notificacionExistente.ModifyDate = DateTime.UtcNow;

            bool result = await notificacionesRepository.Update(notificacionExistente);

            if (!result)
            {
                return BadRequest(new { Message = "Error al actualizar la notificación." });
            }

            return Ok(new { Message = "Notificación actualizada con éxito." });
        }

        // DELETE api/<NotificacionesController>/5
        [HttpDelete("RemoveNotificacion/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await notificacionesRepository.Remove(id);

            if (!result)
            {
                return NotFound(new { Message = "La notificación no fue encontrada." });
            }

            return NoContent();
        }
    }
}
