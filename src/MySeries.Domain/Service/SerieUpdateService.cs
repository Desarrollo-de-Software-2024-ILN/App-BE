using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using MySeries.Series;
using MySeries.Notificaciones;

namespace MySeries.Service
{
    public class SerieUpdateService : DomainService, ISerieUpdateService
    {
        private readonly IRepository<Serie, int> _repository;
        private readonly ISeriesApiService _seriesApiService;
        private readonly INotificationService _notificationService;
      

        public SerieUpdateService 
            (IRepository<Serie, int> repository, 
            ISeriesApiService seriesApiService, 
            INotificationService notificationService)
        {
            _repository = repository;
            _seriesApiService = seriesApiService;
            _notificationService = notificationService;
        }

        public async Task VerificarYActualizarSeriesAsync()
        {
            var series = await _repository.GetListAsync();

            foreach (var serie in series)
            {
                var apiSeries = await _seriesApiService.BuscarSerieAsync(serie.Title, serie.Gender);

                if (apiSeries != null && apiSeries.Length > 0)
                {
                    var apiSerie = apiSeries.FirstOrDefault();

                    // agrega la nueva temporada si hay
                    if (apiSerie.TotalTemporadas > serie.TotalTemporadas)
                    {
                        var nuevaTemporadaNum = serie.TotalTemporadas + 1;
                        var nuevaTemporadaApi = await _seriesApiService.BuscarTemporadaAsync(apiSerie.id, nuevaTemporadaNum);

                        if (nuevaTemporadaApi != null)
                        {
                            var nuevaTemporada = new Temporada
                            {
                                NumTemporada = nuevaTemporadaNum,
                                Episodios = nuevaTemporadaApi.Episodios.Select(e => new Episodio
                                {
                                    Titulo = e.Titulo,
                                    NumEpisodio = e.NumEpisodio,
                                    FechaEstreno = e.FechaEstreno
                                }).ToList()
                            };

                            serie.Temporadas.Add(nuevaTemporada);
                            serie.TotalTemporadas = apiSerie.TotalTemporadas;

                            await _repository.UpdateAsync(serie);

                            //notificar al usuario
                            var tituloNotificacionTemporada = $"Nueva temporada disponible de {serie.Title}";
                            var mensajeNotificacionTemporada = $"La temporada {nuevaTemporadaNum} ya está disponible en {serie.Title}.";

                            var userId = 001; // Suponiendo un usuario por defecto

                            //los usuarios podran elegir si quieren por mail o por pantalla el aviso
                            await _notificationService.CrearNotificacionesAsync(
                                userId, tituloNotificacionTemporada, mensajeNotificacionTemporada, TipoNotificacion.Pantalla);
                            await _notificationService.CrearNotificacionesAsync(
                                userId, tituloNotificacionTemporada, mensajeNotificacionTemporada, TipoNotificacion.Email);
                        }
                    }
                    // Obtener la última temporada 
                    var ultimaTemporada = serie.Temporadas.OrderByDescending(t => t.NumTemporada).FirstOrDefault();
                    if (ultimaTemporada != null)
                    {
                        // Obtener la última temporada desde la API
                        var apiUltimaTemporada = await _seriesApiService.BuscarTemporadaAsync(apiSerie.id, ultimaTemporada.NumTemporada);

                        if (apiUltimaTemporada != null)
                        {
                            // Comparar la cantidad de episodios
                            if (apiUltimaTemporada.Episodios.Count > ultimaTemporada.Episodios.Count)
                            {
                                // Detectar episodios nuevos
                                var episodiosLocales = ultimaTemporada.Episodios.Select(e => e.NumEpisodio).ToHashSet();
                                var episodiosNuevos = apiUltimaTemporada.Episodios
                                    .Where(e => !episodiosLocales.Contains(e.NumEpisodio))
                                    .ToList();

                                if (episodiosNuevos.Any())
                                {
                                    // Lógica para manejar los episodios nuevos
                                    foreach (var episodioNuevo in episodiosNuevos)
                                    {
                                        var nuevoEpisodio = new Episodio
                                        {
                                            Titulo = episodioNuevo.Titulo,
                                            NumEpisodio = episodioNuevo.NumEpisodio,
                                            FechaEstreno = episodioNuevo.FechaEstreno,
                                            TemporadaID = ultimaTemporada.Id
                                        };

                                        // Agregar a la colección de episodios de la temporada local
                                        ultimaTemporada.Episodios.Add(nuevoEpisodio);
                                    }

                                    // Reemplazo de la última temporada
                                    var ultimaTemporadaSerie = serie.Temporadas.OrderByDescending(t => t.NumTemporada).FirstOrDefault();
                                    var listaTemporadas = serie.Temporadas.ToList();
                                    var indiceUltimaTemporadaSerie = listaTemporadas.IndexOf(ultimaTemporadaSerie);
                                    listaTemporadas[indiceUltimaTemporadaSerie] = ultimaTemporada;
                                    serie.Temporadas = listaTemporadas;

                                    await _repository.UpdateAsync(serie);

                                    // Generar y persistir la notificación para la serie
                                    var tituloNotificacion = $"Nuevos episodios en {serie.Title}";
                                    var mensajeNotificacion = $"Se han añadido {episodiosNuevos.Count} nuevos episodios en la serie {serie.Title}.";

                                    var usuarioId = 001; // Suponiendo un usuario por defecto

                                    // Notificar al usuario sobre los nuevos episodios
                                    await _notificationService.CrearNotificacionesAsync(
                                        usuarioId, tituloNotificacion, mensajeNotificacion, TipoNotificacion.Email);
                                    await _notificationService.CrearNotificacionesAsync(
                                        usuarioId, tituloNotificacion, mensajeNotificacion, TipoNotificacion.Pantalla);
                                }
                                }
                            }
                        }                           
                    }
                }
            }
        }
    }
}
