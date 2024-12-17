using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.Domain.Entities;
using MySeries.Users;

namespace MySeries.Series
{
    public class SerieAppService : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto>, ISeriesAppService
    {
        private readonly ISeriesApiService _seriesApiService;
        private readonly IRepository<Serie, int> _repository;
        private readonly ICurrentUserService _currentUser;

        public SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesApiService, ICurrentUserService currentUser) : base(repository)
        {
            _seriesApiService = seriesApiService;
            _repository = repository;
            _currentUser = currentUser;
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
    }
}
