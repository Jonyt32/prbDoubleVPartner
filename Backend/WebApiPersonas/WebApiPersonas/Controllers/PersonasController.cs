using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiPersonas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [HttpPost()]
        public async Task<IActionResult> AddPersona([FromBody] Persona persona)
        {
            if (persona == null)
            {
                return BadRequest("La información de la persona no puede ser nula.");
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var personas = await _personaService.GetAllAsync();
            return Ok(personas);
        }

        /// <summary>
        /// Obtiene una persona por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
                return NotFound();
            return Ok(persona);
        }

        /// <summary>
        /// Consulta personas utilizando el procedimiento almacenado.
        /// </summary>
        [HttpPost("consultar")]
        public async Task<IActionResult> ConsultarConSP([FromBody] PersonaFiltro filtro)
        {
            var personas = await _personaService.ObtenerPersonasConSPAsync(filtro);
            return Ok(personas);
        }

        /// <summary>
        /// Actualiza una persona existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Persona persona)
        {
            if (id != persona.Identificador)
                return BadRequest("El ID de la persona no coincide con el ID de la ruta.");

            var existe = await _personaService.GetByIdAsync(id);
            if (existe == null)
                return NotFound();

            await _personaService.UpdateAsync(persona);
            return NoContent();
        }

        /// <summary>
        /// Elimina una persona por su identificador.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var persona = await _personaService.GetByIdAsync(id);
            if (persona == null)
                return NotFound();

            await _personaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
