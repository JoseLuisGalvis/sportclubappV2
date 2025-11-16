// Data/Interfaces/ISocioRepository.cs
using SportClubApp.Models;
using SportClubApp.Services;

namespace SportClubApp.Data.Interfaces
{
    public interface ISocioRepository
    {
        Task<int> CrearSocioAsync(Socio socio);
        Task<Socio> ObtenerSocioPorIdAsync(int id);
        Task<Socio> ObtenerSocioPorNroAsync(int nroSocio);
        Task<List<Socio>> ObtenerTodosLosSociosAsync();
        Task<bool> ActualizarSocioAsync(Socio socio);
        Task<bool> HabilitarSocioAsync(int nroSocio);
        Task<bool> DeshabilitarSocioAsync(int nroSocio);
        Task<bool> MarcarPagoCompletadoAsync(int nroSocio, string paymentIntentId);
        Task<bool> ExisteSocioPorPersonaIdAsync(int personaId);
        Task<List<Socio>> ObtenerSociosConPagoCompletado();
        Task<Services.SocioInfo> ObtenerSocioParaCarnetAsync(int nroSocio);
        Task<bool> MarcarCarnetEntregadoAsync(int nroSocio, string codigoCarnet);
    }
}