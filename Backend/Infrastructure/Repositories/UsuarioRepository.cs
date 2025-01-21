using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Settings;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IOptions<DatabaseSettings> dbSettings)
        {
            _connectionString = dbSettings.Value.DefaultConnection;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var query = "SELECT Identificador, Usuario, Pass, FechaCreacion FROM Usuarios";
                return await connection.QueryAsync<Usuario>(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var query = "SELECT Identificador, Usuario, Pass, FechaCreacion FROM Usuarios WHERE Identificador = @Id";
                return await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Usuario> GetuserLoginsync(string usuario, string pass) 
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var query = "SELECT Identificador, Usuario, Pass, FechaCreacion FROM Usuarios WHERE Usuario = @Usuario and Pass=@Pass";
                return await connection.QuerySingleOrDefaultAsync<Usuario>(query, new { Usuario = usuario, Pass = pass });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddAsync(Usuario entity)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var query = @"
                INSERT INTO Usuarios (Usuario, Pass, FechaCreacion)
                VALUES (@Usuario, @Pass, @FechaCreacion)";
                await connection.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(Usuario entity)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var query = @"
                UPDATE Usuarios 
                SET Usuario = @Usuario, Pass = @Pass 
                WHERE Identificador = @Identificador";
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
                using var connection = new SqlConnection(_connectionString);
                var query = "DELETE FROM Usuarios WHERE Identificador = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
