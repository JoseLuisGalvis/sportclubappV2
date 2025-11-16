// Data/Repositories/PagoRepository.cs
using MySql.Data.MySqlClient;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;
using System.Data;
using System.Text;

namespace SportClubApp.Data.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly ICuotaRepository _cuotaRepository;

        public PagoRepository(IDatabaseConnection dbConnection, ICuotaRepository cuotaRepository)
        {
            _dbConnection = dbConnection;
            _cuotaRepository = cuotaRepository;
        }

        public async Task<int> CrearPagoAsync(Pago pago)
        {
            if (!pago.Validar(out string mensajeError))
                throw new ArgumentException(mensajeError);

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            // Generar número de comprobante si no existe
            if (string.IsNullOrEmpty(pago.NumeroComprobante))
            {
                pago.NumeroComprobante = pago.GenerarNumeroComprobante();
            }

            const string query = @"
                INSERT INTO pago (fechaPago, monto, metodoPago, cuota_id, socio_id, numero_comprobante, observaciones) 
                VALUES (@fechaPago, @monto, @metodoPago, @cuota_id, @socio_id, @numero_comprobante, @observaciones);
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
            cmd.Parameters.AddWithValue("@monto", pago.Monto);
            cmd.Parameters.AddWithValue("@metodoPago", pago.MetodoPago.ToString());
            cmd.Parameters.AddWithValue("@cuota_id", pago.CuotaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@socio_id", pago.SocioId);
            cmd.Parameters.AddWithValue("@numero_comprobante", pago.NumeroComprobante);
            cmd.Parameters.AddWithValue("@observaciones", pago.Observaciones ?? (object)DBNull.Value);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            pago.Id = id;

            // Si es pago de cuota, marcar la cuota como pagada
            if (pago.CuotaId.HasValue)
            {
                await _cuotaRepository.MarcarCuotaComoPagadaAsync(pago.CuotaId.Value, pago.MetodoPago);
            }

            return id;
        }

        public async Task<Pago> ObtenerPagoPorIdAsync(int id)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, fechaPago, monto, metodoPago, cuota_id, socio_id, numero_comprobante, observaciones 
                FROM pago WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Pago
                {
                    Id = reader.GetInt32("id"),
                    FechaPago = reader.GetDateTime("fechaPago"),
                    Monto = reader.GetDouble("monto"),
                    MetodoPago = Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    CuotaId = reader.IsDBNull("cuota_id") ? null : reader.GetInt32("cuota_id"),
                    SocioId = reader.GetInt32("socio_id"),
                    NumeroComprobante = reader.GetString("numero_comprobante"),
                    Observaciones = reader.IsDBNull("observaciones") ? null : reader.GetString("observaciones")
                };
            }
            return null;
        }

        public async Task<List<Pago>> ObtenerPagosPorSocioAsync(int socioId)
        {
            var pagos = new List<Pago>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, fechaPago, monto, metodoPago, cuota_id, socio_id, numero_comprobante, observaciones 
                FROM pago WHERE socio_id = @socio_id 
                ORDER BY fechaPago DESC";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@socio_id", socioId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var pago = new Pago
                {
                    Id = reader.GetInt32("id"),
                    FechaPago = reader.GetDateTime("fechaPago"),
                    Monto = reader.GetDouble("monto"),
                    MetodoPago = Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    CuotaId = reader.IsDBNull("cuota_id") ? null : reader.GetInt32("cuota_id"),
                    SocioId = reader.GetInt32("socio_id"),
                    NumeroComprobante = reader.GetString("numero_comprobante"),
                    Observaciones = reader.IsDBNull("observaciones") ? null : reader.GetString("observaciones")
                };
                pagos.Add(pago);
            }

            return pagos;
        }

        public async Task<List<Pago>> ObtenerPagosPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var pagos = new List<Pago>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, fechaPago, monto, metodoPago, cuota_id, socio_id, numero_comprobante, observaciones 
                FROM pago 
                WHERE fechaPago BETWEEN @fechaInicio AND @fechaFin
                ORDER BY fechaPago DESC";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Date);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Date.AddDays(1).AddSeconds(-1));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var pago = new Pago
                {
                    Id = reader.GetInt32("id"),
                    FechaPago = reader.GetDateTime("fechaPago"),
                    Monto = reader.GetDouble("monto"),
                    MetodoPago = Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    CuotaId = reader.IsDBNull("cuota_id") ? null : reader.GetInt32("cuota_id"),
                    SocioId = reader.GetInt32("socio_id"),
                    NumeroComprobante = reader.GetString("numero_comprobante"),
                    Observaciones = reader.IsDBNull("observaciones") ? null : reader.GetString("observaciones")
                };
                pagos.Add(pago);
            }

            return pagos;
        }

        public async Task<List<Pago>> ObtenerTodosLosPagosAsync()
        {
            var pagos = new List<Pago>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, fechaPago, monto, metodoPago, cuota_id, socio_id, numero_comprobante, observaciones 
                FROM pago 
                ORDER BY fechaPago DESC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var pago = new Pago
                {
                    Id = reader.GetInt32("id"),
                    FechaPago = reader.GetDateTime("fechaPago"),
                    Monto = reader.GetDouble("monto"),
                    MetodoPago = Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    CuotaId = reader.IsDBNull("cuota_id") ? null : reader.GetInt32("cuota_id"),
                    SocioId = reader.GetInt32("socio_id"),
                    NumeroComprobante = reader.GetString("numero_comprobante"),
                    Observaciones = reader.IsDBNull("observaciones") ? null : reader.GetString("observaciones")
                };
                pagos.Add(pago);
            }

            return pagos;
        }

        public async Task<double> ObtenerTotalPagosPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT COALESCE(SUM(monto), 0) 
                FROM pago 
                WHERE fechaPago BETWEEN @fechaInicio AND @fechaFin";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Date);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Date.AddDays(1).AddSeconds(-1));

            var result = await cmd.ExecuteScalarAsync();
            return result == DBNull.Value ? 0 : Convert.ToDouble(result);
        }

        public async Task<int> RegistrarPagoCuotaAsync(int cuotaId, MetodoPago metodoPago, string observaciones = null)
        {
            // Obtener la cuota
            var cuota = await _cuotaRepository.ObtenerCuotaPorIdAsync(cuotaId);
            if (cuota == null)
                throw new ArgumentException("Cuota no encontrada");

            if (cuota.Estado == EstadoCuota.Pagada)
                throw new InvalidOperationException("La cuota ya está pagada");

            // Crear el pago
            var pago = new Pago(
                cuota.MontoTotal(), // Incluye recargos si está vencida
                metodoPago,
                cuota.SocioId,
                cuotaId,
                observaciones
            );

            return await CrearPagoAsync(pago);
        }

        public Task<int> InsertPagoAsync(Pago pago)
        {
            throw new NotImplementedException();
        }

        public Task<List<Pago>> ObtenerHistorialPagosAsync(int nroSocio)
        {
            throw new NotImplementedException();
        }

        public Task<Pago> GetPagoByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}