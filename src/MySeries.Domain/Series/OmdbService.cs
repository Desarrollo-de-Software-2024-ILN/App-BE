using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MySeries.Series;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace MySeries.Series
{
    public class OmdbService : ISeriesApiService, ITransientDependency
    {
        private const string apiKey = "844b1b8b"; // Reemplaza con tu clave de OMDB
        private const string baseUrl = "http://www.omdbapi.com/";
       // private static readonly string apiKey = "fa5ffac0"; // Reemplaza con tu clave API de OMDb.
       // private static readonly string baseUrl = "http://www.omdbapi.com/";

        // Método principal de búsqueda que controla la lógica de validación
        public async Task<SerieDto[]> BuscarSerieAsync(string title, string genre = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("El campo título es obligatorio para la búsqueda.", nameof(title));
            }

            if (!string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(genre))
            {
                return await BuscarPorTituloAsync(title);
            }

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(genre))
            {
                return await BuscarPorTituloYGeneroAsync(title, genre);
            }

            throw new ArgumentException("No se puede buscar solo por género. El título es obligatorio.");
        }

        private async Task<SerieDto[]> BuscarPorTituloAsync(string title)
        {
            var url = $"{baseUrl}?apikey={apiKey}&s={title}&type=series";
            return await ObtenerSeriesDesdeOmdbAsync(url);
        }

        private async Task<SerieDto[]> BuscarPorTituloYGeneroAsync(string title, string genre)
        {
            var url = $"{baseUrl} ?apikey= {apiKey}&s={title}&type=series";
            var series = await ObtenerSeriesDesdeOmdbAsync(url);

            var seriesFiltradas = new List<SerieDto>();
            foreach (var serie in series)
            {
                if (serie.Generos != null && serie.Generos.Contains(genre, StringComparison.OrdinalIgnoreCase))
                {
                    seriesFiltradas.Add(serie);
                }
            }

            return seriesFiltradas.ToArray();
        }

        private async Task<SerieDto[]> ObtenerSeriesDesdeOmdbAsync(string url)
        {

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Aquí podemos agregar el código para almacenar o procesar el monitoreo, si es necesario.

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                if (json["Response"]?.ToString() == "False")
                {
                    return Array.Empty<SerieDto>();
                }

                var seriesJson = json["Search"];
                if (seriesJson == null)
                {
                    return Array.Empty<SerieDto>();
                }

                var seriesList = new List<SerieDto>();
                foreach (var serie in seriesJson)
                {
                    var serieId = serie["imdbID"]?.ToString();
                    var serieDetails = await ObtenerDetallesSerieAsync(serieId);

                    if (serieDetails != null)
                    {
                        seriesList.Add(serieDetails);
                    }
                }

                return seriesList.ToArray();
            }
        }

        public async Task<TemporadaDto> BuscarTemporadaAsync(string id, int numTemporada)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("El identificador IMDb es obligatorio para buscar una temporada.", nameof(id));
            }

            var url = $"{baseUrl}?apikey={apiKey}&i={id}&season={numTemporada}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                if (json["Response"]?.ToString() == "False")
                {
                    return null;
                }

                var episodiosJson = json["Episodes"];
                if (episodiosJson == null)
                {
                    return null;
                }

                var episodiosList = new List<EpisodioDto>();
                foreach (var episodio in episodiosJson)
                {
                    episodiosList.Add(new EpisodioDto
                    {
                        Titulo = episodio["Title"]?.ToString(),
                        NumEpisodio = int.TryParse(episodio["Episode"]?.ToString(), out var episodioNum) ? episodioNum : 0,
                        FechaEstreno = DateOnly.TryParse(episodio["Released"]?.ToString(), out var fecha) ? fecha : DateOnly.MinValue
                    });
                }

                return new TemporadaDto
                {
                    Titulo = json["Title"]?.ToString(),
                    NumTemporada = int.TryParse(json["Season"]?.ToString(), out var seasonNumber) ? seasonNumber : 0,
                    Episodios = episodiosList
                };
            }
        }


        private async Task<SerieDto> ObtenerDetallesSerieAsync(string id)
        {
            var url = $"{baseUrl}?apikey={apiKey}&i={id}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Aquí podemos agregar el código para almacenar o procesar el monitoreo, si es necesario.

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                return new SerieDto
                {
                    Title = json["Title"]?.ToString(),
                    Generos = json["Genre"]?.ToString(),
                    Tipo = json["Type"]?.ToString(),
                    TotalTemporadas = int.TryParse(json["totalSeasons"]?.ToString(), out var seasons) ? seasons : 0
                };
            }
        }
    }
}