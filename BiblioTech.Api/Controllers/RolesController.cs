using BiblioTech.Api.Models.Roles;
using BiblioTech.API.Models.Notificaciones;
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
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;
        public RolesController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        // GET: api/<RolesController>
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetAll()
        {
            DataResults<List<RolesModel>> result = new DataResults<List<RolesModel>>();
            result = await roleRepository.GetAll();

            if (!result.Sucess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // GET api/<RolesController>/5
        [HttpGet("GetRolesByRole")]
        public async Task<IActionResult> GetByRol(string rol)
        {
            var result = await roleRepository.GetRolesByRole(rol);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron roles para el rol especificado." });
            }

            return Ok(result);
        }


        // GET api/<RolesController>/byId
        [HttpGet("GetRolesById")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await roleRepository.GetRoleById(Id);

            if (result == null || result.Count == 0)
            {
                return NotFound(new { Message = "No se encontraron notificaciones para el autor especificado." });
            }


            return Ok(result);
        }


        // POST api/<RolesController>
        [HttpPost("CreateRole")]
        public async Task<IActionResult> Post([FromBody] RolesCreateDTO rolesCreateDTO)
        {
            bool result = false;
            result = await this.roleRepository.Create(new Domain.Entities.Roles()
            {
                Role = rolesCreateDTO.Role,
                CreationUser = rolesCreateDTO.CreationUser,
                CreationDate = rolesCreateDTO.CreationDate,
            });

            if (!result)
            {
                return BadRequest(new { Message = "Error al crear el rol." });
            }

            return Ok(new { Message = "Rol creada con éxito." });
        }

        // PUT api/<RolesController>/5
        [HttpPut("UpdateRoles/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RolesUpdateDTO rolesUpdateDTO)
        {
            var roleExistente = await roleRepository.GetByCondition(rol => rol.Id == id && rol.isDeleted != true);
            if (roleExistente == null)
            {
                return NotFound(new { Message = "El rol no fue encontrado." });
            }

            roleExistente.Role = rolesUpdateDTO.Role;
            roleExistente.ModifyUser = rolesUpdateDTO.ModifyUser;
            roleExistente.ModifyDate = DateTime.UtcNow;

            bool result = await roleRepository.Update(roleExistente);

            if (!result)
            {
                return BadRequest(new { Message = "Error al actualizar el rol." });
            }

            return Ok(new { Message = "Role actualizada con éxito." });
        }

        // DELETE api/<RolesController>/5
        [HttpDelete("RemoveRole/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await roleRepository.Remove(id);

            if (!result)
            {
                return NotFound(new { Message = "El role no fue encontrado." });
            }

            return NoContent();
        }
    }
}
