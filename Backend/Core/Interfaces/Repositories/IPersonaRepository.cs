using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IPersonaRepository : IRepository<Persona>
    {
        Task<IEnumerable<Persona>> ObtenerPersonasConSPAsync(PersonaFiltro filtro);
    }
}
