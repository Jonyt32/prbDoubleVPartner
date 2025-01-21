using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Settings;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IOptions<DatabaseSettings> dbSettings)
        {
            _connectionString = dbSettings.Value.DefaultConnection;
        }

        // Método privado para obtener la conexión
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            try
            {
                using var connection = GetConnection();
                var query = "SELECT Identificador, NombreUsuario, Contrasena, FechaCreacion FROM Usuarios";
                return await connection.QueryAsync<Usuario>(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los usuarios.", ex);
            }
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            try
            {
                using var connection = GetConnection();
                var query = "SELECT Identificador, NombreUsuario, Contrasena, FechaCreacion FROM Usuarios WHERE Identificador = @Id";
                return await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario por ID.", ex);
            }
        }

        public async Task<Usuario> GetuserLoginsync(string usuario, string pass)
        {
            try
            {
                using var connection = GetConnection();
                var query = "SELECT Identificador, NombreUsuario, Contrasena, FechaCreacion FROM Usuarios WHERE NombreUsuario = @Usuario and Contrasena=@Pass";
                return await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Usuario = usuario, Pass = pass });
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar las credenciales de usuario.", ex);
            }
        }

        public async Task AddAsync(Usuario entity)
        {
            try
            {
                using var connection = GetConnection();
                var query = @"
                INSERT INTO Usuarios (NombreUsuario, Contrasena, FechaCreacion)
                VALUES (@NombreUsuario, @Contrasena, @FechaCreacion)";
                await connection.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el usuario.", ex);
            }
        }

        public async Task UpdateAsync(Usuario entity)
        {
            try
            {
                using var connection = GetConnection();
                var query = @"
                UPDATE Usuarios 
                SET NombreUsuario = @NombreUsuario, Contrasena = @Contrasena 
                WHERE Identificador = @Identificador";
                await connection.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var connection = GetConnection();
                var query = "DELETE FROM Usuarios WHERE Identificador = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario.", ex);
            }
        }
    }
}
