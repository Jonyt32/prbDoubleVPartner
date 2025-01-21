using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IPersonaService
    {
        Task<IEnumerable<Persona>> GetAllAsync();
        Task<Persona> GetByIdAsync(int id);
        Task AddAsync(Persona entity);
        Task UpdateAsync(Persona entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Persona>> ObtenerPersonasConSPAsync(PersonaFiltro filtro);
    }
}
