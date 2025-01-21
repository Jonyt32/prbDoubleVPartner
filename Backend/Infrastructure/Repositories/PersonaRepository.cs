using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using Core.Settings;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace Infrastructure.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly string _connectionString;
        private readonly DatabaseSettings dbSettings;

        public PersonaRepository(IOptions<DatabaseSettings> dbSettings)
        {
            _connectionString = dbSettings.Value.DefaultConnection;
        }

        public async Task<IEnumerable<Persona>> ObtenerPersonasConSPAsync(PersonaFiltro filtro)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parametros = new
                {
                    IDENTIFICADOR = filtro.Identificador,
                    TIPOIDENTIFICACION = filtro.TipoIdentificacion,
                    NUMEROIDENTIFICACION = filtro.NumeroIdentificacion,
                    NOMBRES = filtro.Nombres,
                    APELLIDOS = filtro.Apellidos,
                    EMAIL = filtro.Email,
                    FECHACREACION = filtro.FechaCreacion
                };

                return await connection.QueryAsync<Persona>(
                    "SP_OBTENER_PERSONAS",
                    parametros,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddAsync(Persona entity)
        {
            try
            {
                const string query = @"
                    INSERT INTO Personas 
                    (TipoIdentificacion, NumeroIdentificacion, Nombres, Apellidos, Email, FechaCreacion)
                    VALUES (@TipoIdentificacion, @NumeroIdentificacion, @Nombres, @Apellidos, @Email, @FechaCreacion)";

                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                const string query = "DELETE FROM Personas WHERE Identificador = @Id";
                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            try
            {
                const string query = @"SELECT TipoIdentificacion, NumeroIdentificacion, Nombres, Apellidos, Email, FechaCreacion, 
                                       Identificacion, NombresApellidos FROM Personas";
                using var connection = new SqlConnection(_connectionString);
                return await connection.QueryAsync<Persona>(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public  async Task<Persona> GetByIdAsync(int id)
        {
            try
            {
                const string query = @"SELECT TipoIdentificacion, NumeroIdentificacion, Nombres, Apellidos, Email, FechaCreacion, 
                                       Identificacion, NombresApellidos FROM Personas WHERE Identificador = @Id";
                using var connection = new SqlConnection(_connectionString);
                return await connection.QueryFirstOrDefaultAsync<Persona>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(Persona entity)
        {
            try
            {
                const string query = @"UPDATE Personas SET 
                    TipoIdentificacion = @TipoIdentificacion,
                    NumeroIdentificacion = @NumeroIdentificacion,
                    Nombres = @Nombres,
                    Apellidos = @Apellidos,
                    Email = @Email,
                    FechaCreacion = @FechaCreacion,
                    Identificacion = CONCAT(@TipoIdentificacion, '-', @NumeroIdentificacion),
                    NombresApellidos = CONCAT(@Nombres, ' ', @Apellidos)
                    WHERE Identificador = @Identificador";

                using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
