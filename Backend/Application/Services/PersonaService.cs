﻿using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        /// <summary>
        /// Obtener personas utilizando un procedimiento almacenado.
        /// </summary>
        public async Task<IEnumerable<Persona>> ObtenerPersonasConSPAsync(PersonaFiltro filtro)
        {
            try
            {
                // Validaciones o lógica adicional antes de la llamada al repositorio
                if (filtro == null)
                {
                    filtro = new PersonaFiltro();
                }

                return await _personaRepository.ObtenerPersonasConSPAsync(filtro);
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
                return await _personaRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Persona> GetByIdAsync(int id)
        {
            return await _personaRepository.GetByIdAsync(id);
        }
        /// <summary>
        /// Agregar una nueva persona.
        /// </summary>
        public async Task AddAsync(Persona entity)
        {
            try
            {
                // Validaciones o lógica adicional antes de la llamada al repositorio
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "La persona no puede ser nula.");
                }

                await _personaRepository.AddAsync(entity);
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
                // Validaciones antes de actualizar
                var existingEntity = await _personaRepository.GetByIdAsync(entity.Identificador);
                if (existingEntity == null)
                    throw new KeyNotFoundException("La persona no existe.");

                await _personaRepository.UpdateAsync(entity);
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
                // Validaciones antes de eliminar
                var existingEntity = await _personaRepository.GetByIdAsync(id);
                if (existingEntity == null)
                    throw new KeyNotFoundException("La persona no existe.");

                await _personaRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
