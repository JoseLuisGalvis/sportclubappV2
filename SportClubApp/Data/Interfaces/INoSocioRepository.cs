using SportClubApp.Models;

namespace SportClubApp.Data.Interfaces
{
    public interface INoSocioRepository
    {
        /// <summary>
        /// Inserta un nuevo registro en la tabla NoSocio.
        /// </summary>
        Task<int> InsertarAsync(NoSocio noSocio);

        /// <summary>
        /// Obtiene un NoSocio por ID de persona
        /// </summary>
        Task<NoSocio> ObtenerPorPersonaIdAsync(int personaId);

        /// <summary>
        /// Verifica si existe un NoSocio por persona ID
        /// </summary>
        Task<bool> ExistePorPersonaIdAsync(int personaId);

        /// <summary>
        /// Marca el pago como completado
        /// </summary>
        Task<bool> MarcarPagoCompletadoAsync(int idNoSocio, string paymentIntentId);

        Task<NoSocio> ObtenerNoSocioPorIdAsync(int idNoSocio); 
  
        Task<bool> ActualizarNoSocioAsync(NoSocio noSocio); 
    }
}

