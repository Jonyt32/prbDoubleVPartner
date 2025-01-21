using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IPersonaRepository : IRepository<Persona>
    {
        Task<IEnumerable<Persona>> ObtenerPersonasConSPAsync(PersonaFiltro filtro);
    }
}
