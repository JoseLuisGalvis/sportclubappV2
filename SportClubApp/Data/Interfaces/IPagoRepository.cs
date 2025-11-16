// Data/Interfaces/IPagoRepository.cs
using SportClubApp.Models;

namespace SportClubApp.Data.Interfaces
{
    public interface IPagoRepository
    {
        Task<int> CrearPagoAsync(Pago pago);
        Task<Pago> ObtenerPagoPorIdAsync(int id);
        Task<List<Pago>> ObtenerPagosPorSocioAsync(int socioId);
        Task<List<Pago>> ObtenerPagosPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<List<Pago>> ObtenerTodosLosPagosAsync();
        Task<double> ObtenerTotalPagosPorFechaAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<int> RegistrarPagoCuotaAsync(int cuotaId, MetodoPago metodoPago, string observaciones = null);
    }
}
