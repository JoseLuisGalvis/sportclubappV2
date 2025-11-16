// Data/Interfaces/IFotoRepository.cs
namespace SportClubApp.Data.Interfaces
{
    public interface IFotoRepository
    {
        Task<bool> GuardarFotoSocioAsync(int nroSocio, byte[] fotoBytes, string tipoImagen);
        Task<byte[]> ObtenerFotoSocioAsync(int nroSocio);
        Task<bool> EliminarFotoSocioAsync(int nroSocio);
        Task<bool> SocioTieneFotoAsync(int nroSocio);
        Task<(bool success, string message)> GuardarFotoSocioDesdeImagenAsync(int nroSocio, Image imagen, int anchoMaximo = 400);
        Task<Image> ObtenerFotoSocioComoImagenAsync(int nroSocio);
    }
}
