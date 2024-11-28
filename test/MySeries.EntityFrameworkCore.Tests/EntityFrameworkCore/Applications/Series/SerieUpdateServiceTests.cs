using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MySeries.Series;
using Volo.Abp.Domain.Repositories;
using MySeries.Notificaciones;
using System.Threading;
using System;
using MySeries.Service;

namespace SerializedStalker.Tests.Series
{
    public class SerieUpdateServiceTests
    {
        private readonly Mock<ISeriesApiService> _seriesApiServiceMock;
        private readonly Mock<IRepository<Serie, int>> _serieRepositoryMock;
        private readonly Mock<INotificationService> _notificacionServiceMock;
        private readonly SerieUpdateService _serieUpdateService;

        public SerieUpdateServiceTests()
        {
            _seriesApiServiceMock = new Mock<ISeriesApiService>();
            _serieRepositoryMock = new Mock<IRepository<Serie, int>>();
            _notificacionServiceMock = new Mock<INotificationService>();

            _serieUpdateService = new SerieUpdateService(
                 _serieRepositoryMock.Object,
                _seriesApiServiceMock.Object,
                _notificacionServiceMock.Object);
        }

        [Fact]
        public async Task Should_Update_Series_When_New_Season_Available()
        {
            // Arrange
            var serie = new Serie
            {
                Title = "Test Serie",
                TotalTemporadas = 1,
                Temporadas = new List<Temporada>()
            };
            var seriesList = new List<Serie> { serie };
            var apiSerie = new SerieDto { TotalTemporadas = 2 };

            _serieRepositoryMock.Setup(r => r.GetListAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seriesList);
            _seriesApiServiceMock.Setup(s => s.BuscarSerieAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new[] { apiSerie });

            var nuevaTemporada = new TemporadaDto
            {
                NumTemporada = 2,
                Episodios = new List<EpisodioDto>
        {
            new EpisodioDto { NumEpisodio = 1, Titulo = "Nuevo episodio", FechaEstreno = DateOnly.FromDateTime(System.DateTime.Now) }
        }
            };
            _seriesApiServiceMock.Setup(s => s.BuscarTemporadaAsync(It.IsAny<string>(), 2))
                .ReturnsAsync(nuevaTemporada);

            // Act
            try
            {
                await _serieUpdateService.VerificarYActualizarSeriesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during test execution: {ex.Message}");
                throw; // Vuelve a lanzar la excepción para que falle el test si es necesario
            }

            // Assert
            // Ajustar la verificación para que coincida con la firma del método
            _serieRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Serie>(s => s.TotalTemporadas == 2), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
            _notificacionServiceMock.Verify(n => n.CrearNotificacionesAsync(
                001,
                $"Nueva temporada disponible de {serie.Title}",
                $"La temporada 2 ya está disponible en {serie.Title}.",
                TipoNotificacion.Email), Times.Once);
        }


        [Fact]
        public async Task Should_Notify_When_New_Episodes_Available()
        {
            // Arrange
            var serie = new Serie
            {
                Title = "Test Serie",
                TotalTemporadas = 1,
                Temporadas = new List<Temporada>
                {
                    new Temporada
                    {
                        NumTemporada = 1,
                        Episodios = new List<Episodio>
                        {
                            new Episodio { NumEpisodio = 1, Titulo = "Episodio 1", FechaEstreno = DateOnly.FromDateTime(System.DateTime.Now) }
                        }
                    }
                }
            };
            var seriesList = new List<Serie> { serie };
            var apiSerie = new SerieDto { TotalTemporadas = 1 };

            _serieRepositoryMock.Setup(r => r.GetListAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(seriesList);
            _seriesApiServiceMock.Setup(s => s.BuscarSerieAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new[] { apiSerie });

            var apiTemporada = new TemporadaDto
            {
                NumTemporada = 1,
                Episodios = new List<EpisodioDto>
                {
                    new EpisodioDto { NumEpisodio = 1, Titulo = "Episodio 1", FechaEstreno = DateOnly.FromDateTime(System.DateTime.Now) },
                    new EpisodioDto { NumEpisodio = 2, Titulo = "Nuevo Episodio 2", FechaEstreno = DateOnly.FromDateTime(System.DateTime.Now) }
                }
            };
            _seriesApiServiceMock.Setup(s => s.BuscarTemporadaAsync(It.IsAny<string>(), 1))
                .ReturnsAsync(apiTemporada);

            // Act
            await _serieUpdateService.VerificarYActualizarSeriesAsync();

            // Assert
            _notificacionServiceMock.Verify(n => n.CrearNotificacionesAsync(
                001,
                $"Nuevos episodios en {serie.Title}",
                "Se han añadido 1 nuevos episodios en la serie Test Serie.",
                TipoNotificacion.Email), Times.Once);
        }
    }
}
