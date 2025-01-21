using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Serilog; // Importar Serilog para logging
using EnviarEmailprbJony;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<PersonaService> _logger; // Inyectar el logger

        public PersonaService(IPersonaRepository personaRepository, IUsuarioRepository usuarioRepository, ILogger<PersonaService> logger)
        {
            _personaRepository = personaRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger; // Asignar el logger
        }

        /// <summary>
        /// Obtener personas utilizando un procedimiento almacenado.
        /// </summary>
        public async Task<IEnumerable<Persona>> ObtenerPersonasConSPAsync(PersonaFiltro filtro)
        {
            try
            {
                _logger.LogInformation("Iniciando la obtención de personas con filtro.");

                if (filtro == null)
                {
                    filtro = new PersonaFiltro();
                }

                var personas = await _personaRepository.ObtenerPersonasConSPAsync(filtro);

                _logger.LogInformation("Consulta realizada con éxito, se obtuvieron {Count} personas.", personas.Count());
                return personas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener personas con filtro.");
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando la consulta para obtener todas las personas.");
                var personas = await _personaRepository.GetAllAsync();
                _logger.LogInformation("Consulta completada exitosamente, se obtuvieron {Count} personas.", personas.Count());
                return personas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las personas.");
                throw new Exception(ex.Message);
            }
        }

        public async Task<Persona> GetByIdAsync(int id)
        {
            _logger.LogInformation("Iniciando la consulta para obtener la persona con ID {Id}.", id);
            var persona = await _personaRepository.GetByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("No se encontró la persona con ID {Id}.", id);
            }
            return persona;
        }

        /// <summary>
        /// Agregar una nueva persona.
        /// </summary>
        public async Task AddAsync(Persona entity)
        {
            try
            {
                _logger.LogInformation("Iniciando la creación de una nueva persona.");

                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "La persona no puede ser nula.");
                }

                await _personaRepository.AddAsync(entity);

                // Crear un nuevo usuario
                Usuario newuser = new Usuario()
                {
                    FechaCreacion = DateTime.Now,
                    NombreUsuario = entity.Email,
                    Contrasena = entity.NumeroIdentificacion
                };
                await _usuarioRepository.AddAsync(newuser);

                // Enviar correo
                EmailSenderService sender = new EmailSenderService();
                bool result = await sender.SendEmailAsync(entity.Email,
                    "Creación de usuario", $"Tu usuario es: {entity.Email} y el pass es {entity.NumeroIdentificacion}");

                if (result)
                {
                    _logger.LogInformation("Correo de confirmación enviado exitosamente a {Email}.", entity.Email);
                }
                else
                {
                    _logger.LogWarning("Error al enviar el correo a {Email}.", entity.Email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la persona.");
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(Persona entity)
        {
            try
            {
                _logger.LogInformation("Iniciando la actualización de la persona con ID {Id}.", entity.Identificador);

                var existingEntity = await _personaRepository.GetByIdAsync(entity.Identificador);
                if (existingEntity == null)
                {
                    _logger.LogWarning("La persona con ID {Id} no existe.", entity.Identificador);
                    throw new KeyNotFoundException("La persona no existe.");
                }

                await _personaRepository.UpdateAsync(entity);
                _logger.LogInformation("Persona con ID {Id} actualizada exitosamente.", entity.Identificador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la persona con ID {Id}.", entity.Identificador);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando la eliminación de la persona con ID {Id}.", id);

                var existingEntity = await _personaRepository.GetByIdAsync(id);
                if (existingEntity == null)
                {
                    _logger.LogWarning("La persona con ID {Id} no existe.", id);
                    throw new KeyNotFoundException("La persona no existe.");
                }

                await _personaRepository.DeleteAsync(id);
                _logger.LogInformation("Persona con ID {Id} eliminada exitosamente.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la persona con ID {Id}.", id);
                throw new Exception(ex.Message);
            }
        }
    }
}
