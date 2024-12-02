using MySeries.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace MySeries.Series
{
    public class SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesApiService, ICurrentUserService currentUserService, IObjectMapper objectMapper) : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto, CreateUpdateSerieDto>(repository), ISeriesAppService
    {
        private readonly ISeriesApiService _seriesApiService = seriesApiService;
        private readonly IRepository<Serie, int> _repository = repository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IObjectMapper _objectMapper = objectMapper;

        public async Task<SerieDto[]> BuscarSerieAsync(string title, string gender = null)
        {
            return await _seriesApiService.BuscarSerieAsync(title, gender);
        }

        //metodo para temporadas
        public async Task<TemporadaDto> BuscarTemporadaAsync(string id, int NumTemporada)
        {
            return await _seriesApiService.BuscarTemporadaAsync(id, NumTemporada);
        }

        public async Task CalificarSerieAsync(CalificacionDto input)
        {
            // Obtener la serie del repositorio
            var serie = await _repository.GetAsync(input.SerieId) ?? throw new EntityNotFoundException(typeof(Serie), input.SerieId);

            // Obtener el ID del usuario actual
            var userIdActual = _currentUserService.GetCurrentUserId();
            if (!userIdActual.HasValue)
            {
                throw new InvalidOperationException("User ID cannot be null");
            }

            // Un usuario solo puede calificar las series relacionadas a él
            if (serie.CreatorId != userIdActual.Value)
            {
                throw new UnauthorizedAccessException("No puedes calificar esta serie.");
            }

            // Crear la nueva calificación
            var calificacion = new Calificacion
            {
                CalificacionNota = input.calificacion,
                Comentario = input.comentario,
                FechaCalificacion = DateTime.Now, // Asegúrate de asignar la fecha de creación
                SerieId = input.SerieId,
                UsuarioId = userIdActual.Value // Asigna el ID del usuario actual
            };

            // Agregar la calificación a la serie
            serie.Calificaciones.Add(calificacion);

            // Actualizar la serie en el repositorio
            await _repository.UpdateAsync(serie);
        }
        // Nuevo método para persistir las series en la base de datos
        public async Task PersistirSeriesAsync(SerieDto[] seriesDto)
        {
            var seriesExistentes = await _repository.GetListAsync(); // Obtener todas las series //No esta devolviendo GetListAsync nada

            seriesExistentes ??= [];

            foreach (var serieDto in seriesDto)
            {
                // Comprobación para evitar excepciones al acceder a propiedades de un objeto que podría ser null
                if (serieDto == null) continue;
                var userIdActual = _currentUserService.GetCurrentUserId();
                var serieExistente = seriesExistentes.FirstOrDefault(s => s.Id == serieDto.Id && s.CreatorId == userIdActual);

                if (serieExistente == null)
                {
                    // Crear nueva serie                 
                    // Utilizar mappers nuevaSerie
                    var nuevaSerie = _objectMapper.Map<SerieDto, Serie>(serieDto);

                    if (serieDto.Temporadas != null)
                    {
                        foreach (var temporadaDto in serieDto.Temporadas)
                        {
                            var nuevaTemporada = _objectMapper.Map<TemporadaDto, Temporada>(temporadaDto);
                            nuevaSerie.Temporadas.Add(nuevaTemporada);
                        }
                    }

                    // Persistir la nueva serie en la base de datos
                    await _repository.InsertAsync(nuevaSerie);
                }
                else
                {
                    // Actualizar la serie existente con nueva información
                    serieExistente.TotalTemporadas = serieDto.TotalTemporadas;
                    await _repository.UpdateAsync(serieExistente);
                }
            }
        }
    }
}