using Core.Entities;

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
