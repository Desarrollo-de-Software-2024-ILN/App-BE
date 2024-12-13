using MySeries.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MySeries.Api
{
    public class OmdbApiService : ISeriesApiService, ITransientDependency
    {
        private const string Key = "844b1b8b";
        private const string Url = "http://www.omdbapi.com/";
        public async Task<SerieDto[]> BuscarSerieAsync(string Title, string Genre = null)
        {
           
            if (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Genre))
            {
                return await BuscarPorTituloYGeneroAsync(Title, Genre);
            }

            if (!string.IsNullOrWhiteSpace(Title) && string.IsNullOrWhiteSpace(Genre))
            {
                return await BuscarPorTituloAsync(Title);
            }

            throw new ArgumentException("Obligatorio colocar Título", nameof(Title));
        }

        private async Task<SerieDto[]> BuscarPorTituloYGeneroAsync(string Title, String Genre)
        {
            var url = $"{Url}?apikey={Key}&s={Title}&type=series";
            var series = await ObtenerSeriesApiAsync(url);

            var seriesFiltradas = new List<SerieDto>();
            foreach (var serie in series)
            {
                if (serie.Genre != null && serie.Genre.Contains(Genre, StringComparison.OrdinalIgnoreCase))
                {
                    seriesFiltradas.Add(serie);
                }
            }

            return seriesFiltradas.ToArray();
        }

        private async Task<SerieDto[]> BuscarPorTituloAsync(string Title)
        {
            var url = $"{Url}?apikey={Key}&s={Title}&type=series";
            return await ObtenerSeriesApiAsync(url);
        }

        private async Task<SerieDto[]> ObtenerSeriesApiAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                
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
                    var serieID = serie["imbdID"]?.ToString();
                    var seriesDetalles = await ObtenerDetallesSerieAsync(serieID);

                    if (seriesDetalles != null)
                    {
                        seriesList.Add(seriesDetalles);
                    }
                }

                return seriesList.ToArray();
            }
        }
        private async Task<SerieDto> ObtenerDetallesSerieAsync(string serieID)
        {
            var url = $"{Url}?apikey={Key}&i={serieID}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonResponse);

                return new SerieDto
                {
                    Title = json["Title"]?.ToString(),
                    Descripcion = json["Plot"]?.ToString(),
                    Genre = json["Genre"]?.ToString(),
                    idSerie = json["idSerie"]?.ToString()
                };
            }
        }
    }
}