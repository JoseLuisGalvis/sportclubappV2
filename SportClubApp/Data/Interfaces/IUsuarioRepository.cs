// Data/Interfaces/IUsuarioRepository.cs
using SportClubApp.Models;

namespace SportClubApp.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        // Métodos principales
        Task<int> CrearUsuarioAsync(Usuario usuario, string password);
        Task<Usuario> ObtenerUsuarioPorIdAsync(int id);
        Task<Usuario> ObtenerUsuarioPorUsernameAsync(string username);
        Task<bool> ValidarCredencialesAsync(string username, string password);
        Task<bool> ActualizarUsuarioAsync(Usuario usuario);
        Task<bool> CambiarPasswordAsync(int usuarioId, string nuevaPassword);
        Task<bool> ExisteUsernameAsync(string username, int? excludeId = null);
        Task<List<Usuario>> ObtenerTodosLosUsuariosAsync();
        Task<bool> ActivarUsuarioAsync(int usuarioId);
        Task<bool> DesactivarUsuarioAsync(int usuarioId);

        // Métodos específicos para administradores
        Task<List<Usuario>> GetAdministradoresAsync();
        Task<bool> EliminarAdministradorAsync(int id);
        Task<bool> ActualizarAdministradorAsync(int id, string nuevoUsername, string nuevaPassword = null);
    }
}