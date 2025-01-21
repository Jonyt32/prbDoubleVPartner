using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> GetByIdAsync(int id);
        Task<Usuario> GetuserLoginsync(string usuario, string pass);
        Task AddAsync(Usuario entity);
        Task UpdateAsync(Usuario entity);
        Task DeleteAsync(int id);
    }
}
