using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiPersonas.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    //[Authorize]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _personaService;

        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        /// <summary>
        /// Agregar una nueva persona.
        /// </summary>
        [HttpPost("CrearPersona")]
        public async Task<IActionResult> AddPersona([FromBody] Persona persona)
        {
            string mensaje = await ValidarPersona(persona);
            if (!string.IsNullOrEmpty(mensaje))
            {
                return BadRequest(mensaje);
            }

            await _personaService.AddAsync(persona);
            return CreatedAtAction(nameof(GetPersonasBySP), new { id = persona.Identificador }, persona);
        }

        /// <summary>
        /// Consultar personas usando el procedimiento almacenado.
        /// </summary>
        [HttpGet("ConsultarPorSP")]
        public async Task<IActionResult> GetPersonasBySP([FromQuery] PersonaFiltro filtro)
        {
            var personas = await _personaService.ObtenerPersonasConSPAsync(filtro);

            if (personas == null || !personas.Any())
            {
                return NotFound("No se encontraron personas con los filtros proporcionados.");
            }

            return Ok(personas);
        }

        /// <summary>
        /// Obtiene todas las personas.
        /// </summary>
        [HttpGet("ListarPersona")]
        public async Task<IActionResult> GetAll()
        {
            var personas = await _personaService.GetAllAsync();
            return Ok(personas);
        }

        /// <summary>
        /// Obtiene una persona por su identificador.
        /// </summary>
        [HttpGet("ConsultarPersona")]
        public async Task<IActionResult> GetById(int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
                return NotFound();
            return Ok(persona);
        }


        /// <summary>
        /// Actualiza una persona existente.
        /// </summary>
        [HttpPut("ActualizarPersona")]
        public async Task<IActionResult> Update([FromBody] Persona persona)
        {
            string mensaje = await ValidarPersona(persona);
            if (!string.IsNullOrEmpty(mensaje)) 
            {
                return BadRequest(mensaje);
            }

            var existe = await _personaService.GetByIdAsync(persona.Identificador);
            if (existe == null)
                return NotFound($"No existe la persona con el identificador {persona.Identificador}");

            await _personaService.UpdateAsync(persona);
            return NoContent();
        }

        /// <summary>
        /// Elimina una persona por su identificador.
        /// </summary>
        [HttpDelete("EliminarPersona")]
        public async Task<IActionResult> Delete(int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
                return NotFound();

            await _personaService.DeleteAsync(id);
            return NoContent();
        }

        private async Task<string> ValidarPersona(Persona persona) 
        {
            string mensaje = string.Empty;
            if (persona == null)
            {
                mensaje = "La información de la persona no puede ser nula.";
            }

            if (persona.Email == string.Empty || persona.NumeroIdentificacion == string.Empty || persona.TipoIdentificacion == string.Empty)
            {
                mensaje = "Los campos Email, Numero identificaicion, Tipo de identificaion son obligatorios";
            }

            return mensaje;
        }
    }
}
