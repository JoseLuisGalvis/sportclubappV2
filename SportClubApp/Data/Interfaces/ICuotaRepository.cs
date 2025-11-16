// Data/Interfaces/ICuotaRepository.cs
using SportClubApp.Models;

namespace SportClubApp.Data.Interfaces
{
    public interface ICuotaRepository
    {
        Task<int> CrearCuotaAsync(Cuota cuota);
        Task<Cuota> ObtenerCuotaPorIdAsync(int id);
        Task<List<Cuota>> ObtenerCuotasPorSocioAsync(int socioId);
        Task<List<Cuota>> ObtenerCuotasVencidasAsync();
        Task<List<Cuota>> ObtenerCuotasPendientesAsync();
        Task<bool> ActualizarCuotaAsync(Cuota cuota);
        Task<bool> MarcarCuotaComoPagadaAsync(int cuotaId, MetodoPago metodoPago);
        Task<bool> TieneCuotasVencidasAsync(int socioId);
        Task<int> ActualizarEstadosCuotasVencidasAsync();
        Task<int> GenerarCuotasMensualesAsync(DateTime mesAnio, double monto);
        Task<List<Cuota>> ObtenerCuotasQueVencenHoyAsync();

        Task<List<Cuota>> ObtenerCuotasImpagasAsync();
    }
}
