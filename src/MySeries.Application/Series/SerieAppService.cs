using Microsoft.AspNetCore.Authorization;
using MySeries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Series
{
    [Authorize]
    public class SerieAppService : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto, CreateUpdateSerieDto>, ISeriesAppService
    {
        private readonly ISeriesApiService _seriesApiService;
        private readonly IRepository<Serie, int> _repository;
        private readonly ICurrentUserService _currentUserService;

        public SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesApiService, ICurrentUserService currentUserService)
            : base(repository)
        {
            _seriesApiService = seriesApiService;
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<ICollection<SerieDto>> SearchAsync(string? title, string? gender)
        {
            return await _seriesApiService.GetSeriesAsync(title, gender);
        }

        public async Task CalificarSerieAsync(CalificacionDto input)
        {
            // Obtener la serie del repositorio
            var serie = await _repository.GetAsync(input.SerieId);
            if (serie == null)
            {
                throw new EntityNotFoundException(typeof(Serie), input.SerieId);
            }

            // Obtener el ID del usuario
            var UserId = _currentUserService.GetCurrentUserId();
            if (!UserId.HasValue)
            {
                throw new InvalidOperationException("User ID cannot be null");
            }

            // Un usuario solo puede calificar sus series
            if (serie.CreatorId != UserId.Value)
            {
                throw new UnauthorizedAccessException("No puedes calificar esta serie.");
            }

            // Crear la calificación
            var calificacion = new Calificacion
            {
                calificacion = input.calificacion,
                comentario = input.comentario,
                FechaCalificacion = DateTime.Now, // Asegúrate de asignar la fecha de creación
                SerieId = input.SerieId,
                UsuarioId = UserId.Value // Asigna el ID del usuario actua
            };

            // Agregar la calificación
            serie.calificaciones.Add(calificacion);

            // Actualizar la serie en el repositorio
            await _repository.UpdateAsync(serie);
        }
    }
}
