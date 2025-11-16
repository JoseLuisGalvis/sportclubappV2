// Data/Interfaces/IPersonaRepository.cs
using SportClubApp.Models;

namespace SportClubApp.Data.Interfaces
{
    public interface IPersonaRepository
    {
        Task<bool> ExistePersonaPorDniAsync(string dni);
        Task<int> CrearPersonaAsync(Persona persona);
        Task<Persona> ObtenerPersonaPorIdAsync(int id);
        Task<bool> ActualizarPersonaAsync(Persona persona);
        Task<List<Persona>> ObtenerTodasLasPersonasAsync();
    }
}
