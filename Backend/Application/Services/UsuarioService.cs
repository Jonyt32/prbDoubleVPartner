using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger; // Correcto: ILogger<NombreDeClase>

        // Inyección de dependencias
        public UsuarioService(IUsuarioRepository usuarioRepository, ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger; // Asignar el logger a la variable privada
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando la consulta para obtener todos los usuarios.");
                var usuarios = await _usuarioRepository.GetAllAsync();
                _logger.LogInformation("Consulta completada exitosamente.");
                return usuarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando la consulta para obtener el usuario con ID {Id}.", id);
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                {
                    _logger.LogWarning("No se encontró el usuario con ID {Id}.", id);
                }
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID {Id}.", id);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> GetuserLoginsync(string usuario, string pass)
        {
            try
            {
                _logger.LogInformation("Iniciando la autenticación para el usuario {Usuario}.", usuario);
                var result = await _usuarioRepository.GetuserLoginsync(usuario, pass);
                if (result == null)
                {
                    _logger.LogWarning("Autenticación fallida para el usuario {Usuario}.", usuario);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar al usuario {Usuario}.", usuario);
                throw new Exception(ex.Message);
            }
        }

        public async Task AddAsync(Usuario entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _logger.LogInformation("Iniciando la creación de un nuevo usuario.");
                await _usuarioRepository.AddAsync(entity);
                _logger.LogInformation("Nuevo usuario creado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar un nuevo usuario.");
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(Usuario entity)
        {
            try
            {
                _logger.LogInformation("Iniciando la actualización del usuario con ID {Id}.", entity.Identificador);
                var existingEntity = await _usuarioRepository.GetByIdAsync(entity.Identificador);
                if (existingEntity == null)
                    throw new KeyNotFoundException("El usuario no existe.");

                await _usuarioRepository.UpdateAsync(entity);
                _logger.LogInformation("Usuario con ID {Id} actualizado exitosamente.", entity.Identificador);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el usuario con ID {Id}.", entity.Identificador);
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando la eliminación del usuario con ID {Id}.", id);
                var existingEntity = await _usuarioRepository.GetByIdAsync(id);
                if (existingEntity == null)
                    throw new KeyNotFoundException("El usuario no existe.");

                await _usuarioRepository.DeleteAsync(id);
                _logger.LogInformation("Usuario con ID {Id} eliminado exitosamente.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el usuario con ID {Id}.", id);
                throw new Exception(ex.Message);
            }
        }
    }
}
