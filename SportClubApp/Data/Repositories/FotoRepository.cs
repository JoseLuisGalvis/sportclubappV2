// Data/Repositories/FotoRepository.cs
using MySql.Data.MySqlClient;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Data.Utils;
using System.Drawing.Imaging;

namespace SportClubApp.Data.Repositories
{
    public class FotoRepository : IFotoRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public FotoRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> GuardarFotoSocioAsync(int nroSocio, byte[] fotoBytes, string tipoImagen)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                const string query = @"
                    UPDATE socio 
                    SET foto_carnet = @foto_carnet, foto_tipo = @foto_tipo 
                    WHERE nroSocio = @nroSocio";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@foto_carnet", fotoBytes);
                cmd.Parameters.AddWithValue("@foto_tipo", tipoImagen);
                cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al guardar foto del socio {nroSocio}: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> ObtenerFotoSocioAsync(int nroSocio)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                const string query = "SELECT foto_carnet FROM socio WHERE nroSocio = @nroSocio";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

                var result = await cmd.ExecuteScalarAsync();
                return result is byte[] bytes ? bytes : null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener foto del socio {nroSocio}: {ex.Message}", ex);
            }
        }

        public async Task<bool> EliminarFotoSocioAsync(int nroSocio)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                const string query = @"
                    UPDATE socio 
                    SET foto_carnet = NULL, foto_tipo = NULL 
                    WHERE nroSocio = @nroSocio";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al eliminar foto del socio {nroSocio}: {ex.Message}", ex);
            }
        }

        public async Task<bool> SocioTieneFotoAsync(int nroSocio)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                const string query = "SELECT foto_carnet FROM socio WHERE nroSocio = @nroSocio";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

                var result = await cmd.ExecuteScalarAsync();
                return result != null && result != DBNull.Value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al verificar foto del socio {nroSocio}: {ex.Message}", ex);
            }
        }

        public async Task<(bool success, string message)> GuardarFotoSocioDesdeImagenAsync(int nroSocio, Image imagen, int anchoMaximo = 400)
        {
            try
            {
                // Redimensionar y optimizar imagen
                var imagenOptimizada = ImageHelper.RedimensionarImagen(imagen, anchoMaximo, anchoMaximo);
                var bytes = ImageHelper.ConvertirImagenABytes(imagenOptimizada, ImageFormat.Jpeg);

                // Guardar en base de datos
                var resultado = await GuardarFotoSocioAsync(nroSocio, bytes, "image/jpeg");

                return resultado ?
                    (true, "Foto guardada exitosamente") :
                    (false, "No se pudo guardar la foto");
            }
            catch (Exception ex)
            {
                return (false, $"Error al procesar imagen: {ex.Message}");
            }
        }

        public async Task<Image> ObtenerFotoSocioComoImagenAsync(int nroSocio)
        {
            try
            {
                var bytes = await ObtenerFotoSocioAsync(nroSocio);
                return bytes != null ? ImageHelper.ConvertirBytesAImagen(bytes) : null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener imagen del socio {nroSocio}: {ex.Message}", ex);
            }
        }
    }
}
