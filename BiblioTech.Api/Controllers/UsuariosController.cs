using BiblioTech.Api.Models.Roles;
using BiblioTech.Api.Models.Usuarios;
using BiblioTech.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Interfaces;
using Persistence.Models;
using Persistence.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BiblioTech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosRepository usuariosRepository;
        public UsuariosController(IUsuariosRepository usuariosRepository)
        {
            this.usuariosRepository = usuariosRepository;
        }

        // GET: api/<UsuariosController>
        [HttpGet("GetUsuarios")]
        public async Task<IActionResult> GetAll()
        {
            DataResults<List<UsuariosModel>> result = new DataResults<List<UsuariosModel>>();
            result = await usuariosRepository.GetAll();

            if (!result.Sucess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET api/<UsuariosController>/5
        [HttpGet("GetUsuariosById")]
        public async Task<IActionResult> GetUsuariosById(int Id)
        {
            var result = await usuariosRepository.GetUsuarioById(Id);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraró el usuario especificado." });
            }


            return Ok(result);
        }

        [HttpGet("GetRolesByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await usuariosRepository.GetUsuariosByName(name);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraró el usuario especificado." });
            }


            return Ok(result);
        }

        [HttpGet("GetRolesByRoleName")]
        public async Task<IActionResult> GetByRoleName(string name)
        {
            var result = await usuariosRepository.GetUsuariosByRoleName(name);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraró el usuario especificado." });
            }


            return Ok(result);
        }


        // POST api/<UsuariosController>
        [HttpPost("CreateUsuario")]
        public async Task<IActionResult> Post([FromBody] UsuariosCreateDTO usuariosCreateDTO)
        {
            bool result = false;
            result = await this.usuariosRepository.Create(new Domain.Entities.Usuarios()
            {
                Name = usuariosCreateDTO.Name,
                eMail = usuariosCreateDTO.eMail,
                Password = usuariosCreateDTO.Password,
                RoleName = usuariosCreateDTO.RoleName,
                RoleId = usuariosCreateDTO.RoleId,
                CreationUser = usuariosCreateDTO.CreationUser,
                CreationDate = usuariosCreateDTO.CreationDate,
            });

            if (!result)
            {
                return BadRequest(new { Message = "Error al crear el usuario." });
            }

            return Ok(new { Message = "usuario creado con éxito." });
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("UpdateRoles/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UsuariosUpdateDTO usuarioToUpdate)
        {
            var usuarioExistente = await usuariosRepository.GetByCondition(us =>us.Id == id && us.isDeleted != true);
            if (usuarioExistente == null)
            {
                return NotFound(new { Message = "El usuario no fue encontrado." });
            }

            usuarioExistente.Name = usuarioToUpdate.Name;
            usuarioExistente.eMail = usuarioToUpdate.eMail;
            usuarioExistente.Password = usuarioToUpdate.Password;
            usuarioExistente.RoleName = usuarioToUpdate.RoleName;
            usuarioExistente.ModifyUser = usuarioToUpdate.ModifyUser;
            usuarioExistente.ModifyDate = DateTime.UtcNow;

            bool result = await usuariosRepository.Update(usuarioExistente);

            if (!result)
            {
                return BadRequest(new { Message = "Error al actualizar el rol." });
            }

            return Ok(new { Message = "Role actualizada con éxito." });
        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("RemoveUsuario/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await usuariosRepository.Remove(id);

            if (!result)
            {
                return NotFound(new { Message = "El usuario no fue encontrado." });
            }

            return NoContent();
        }
    }
}
