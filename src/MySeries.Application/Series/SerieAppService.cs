using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.ObjectMapping;
using MySeries.Users;
using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;

namespace MySeries.Series
{
    [Authorize]
    public class SerieAppService : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto, CreateUpdateSerieDto>, ISeriesAppService
    {
        private readonly ISeriesApiService _seriesApiService;
        private readonly IRepository<Serie, int> _repository;
        private readonly ICurrentUserService _currentUser;
        private readonly IObjectMapper _mapper;


        public SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesApiService, ICurrentUserService currentUser, IObjectMapper mapper) : base(repository)
        {
            _seriesApiService = seriesApiService;
            _repository = repository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<SerieDto[]> BuscarSerieAsync(string Title, string Genre = null)
        {
            return await _seriesApiService.BuscarSerieAsync(Title, Genre);
        }

        public async Task CalificarSerieAsync(CalificationDto input)
        {
            var serie = await _repository.GetAsync(input.IdSerie);
            if (serie == null)
            {
                throw new EntityNotFoundException(typeof(Serie),input.IdSerie);
            }

            var userId = _currentUser.GetCurrentUserID();
            if (!userId.HasValue)
            {
                throw new InvalidOperationException("Obligatorio usuario");
            }

            if(serie.CreatorId !=  userId.Value)
            {
                throw new UnauthorizedAccessException("No puedes calificar esta serie");
            }

            var calification = new Calification
            {
                Nota = input.Nota,
                Comentario = input.Comentario,
                FechaCreada = DateTime.Now,
                IdSerie = input.IdSerie,
                User = userId.Value,
            };

            serie.Califications.Add(calification);

            await _repository.UpdateAsync(serie);
        }

        public async Task ObtenerSeriesAsync(SerieDto[] serieDtos)
        {
            var listSerieaux = await _repository.GetListAsync();
            if (listSerieaux == null)
            {
                listSerieaux = new List<Serie>();
            }

            foreach (var serieDto  in serieDtos)
            {
                if (serieDto == null) continue;
                var userId = _currentUser.GetCurrentUserID();
                var serieaux = listSerieaux.FirstOrDefault(s => s.IdSerie == serieDto.IdSerie & s.CreatorId == userId);

                if (serieaux == null)
                {
                    var newSerie = _mapper.Map<SerieDto,Serie>(serieDto);
                    await _repository.InsertAsync(newSerie);
                }
                else
                {
                    await _repository.UpdateAsync(serieaux);
                }
            }
        }
    }
}
