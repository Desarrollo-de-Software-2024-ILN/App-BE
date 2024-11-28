using System.Threading.Tasks;

namespace MySeries.Series
{
    public interface ISerieUpdateService
    {
        Task VerificarYActualizarSeriesAsync();
    }
}