using Application.Services.Authentication;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiPersonas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtService _jwtService;
        public UsuarioController(IUsuarioService usuarioService, IJwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        [HttpGet("Listarusuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("ObtenerUsuario")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpPost("CrearUsuario")]
        public async Task<ActionResult> Add([FromBody] Usuario usuario)
        {
            await _usuarioService.AddAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Identificador }, usuario);
        }

        [HttpPut("ActualizarUsuario")]
        public async Task<ActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Identificador)
                return BadRequest();

            await _usuarioService.UpdateAsync(usuario);
            return NoContent();
        }

        [HttpDelete("EliminarUsuario")]
        public async Task<ActionResult> Delete(int id)
        {
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("LoginUsuario")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var usuario = await _usuarioService.GetuserLoginsync(loginRequest.Username, loginRequest.Password);
            if (usuario != null) // Reemplaza con tu lógica de validación
            {
                var token = _jwtService.GenerateToken(usuario.Identificador.ToString(), usuario.NombreUsuario);

                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password");
        }
    }
}
