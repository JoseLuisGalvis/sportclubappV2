// Data/Repositories/CuotaRepository.cs
using MySql.Data.MySqlClient;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;
using System.Data;
using System.Text;

namespace SportClubApp.Data.Repositories
{
    public class CuotaRepository : ICuotaRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public CuotaRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CrearCuotaAsync(Cuota cuota)
        {
            if (!cuota.Validar(out string mensajeError))
                throw new ArgumentException(mensajeError);

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                INSERT INTO cuota (socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, fecha_creacion) 
                VALUES (@socio_id, @monto, @fechaVencimiento, @estado, @tipoDeCuota, @metodoPago, @fecha_creacion);
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@socio_id", cuota.SocioId);
            cmd.Parameters.AddWithValue("@monto", cuota.Monto);
            cmd.Parameters.AddWithValue("@fechaVencimiento", cuota.FechaVencimiento);
            cmd.Parameters.AddWithValue("@estado", cuota.Estado.ToString());
            cmd.Parameters.AddWithValue("@tipoDeCuota", cuota.TipoCuota.ToString());
            cmd.Parameters.AddWithValue("@metodoPago", cuota.MetodoPago?.ToString() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fecha_creacion", cuota.FechaCreacion);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            cuota.Id = id;
            return id;
        }

        public async Task<Cuota> ObtenerCuotaPorIdAsync(int id)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, fecha_creacion 
                FROM cuota WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Cuota
                {
                    Id = reader.GetInt32("id"),
                    SocioId = reader.GetInt32("socio_id"),
                    Monto = reader.GetDouble("monto"),
                    FechaVencimiento = reader.GetDateTime("fechaVencimiento"),
                    Estado = Enum.Parse<EstadoCuota>(reader.GetString("estado")),
                    TipoCuota = Enum.Parse<TipoCuota>(reader.GetString("tipoDeCuota")),
                    MetodoPago = reader.IsDBNull("metodoPago") ? null : Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
            }
            return null;
        }

        public async Task<List<Cuota>> ObtenerCuotasPorSocioAsync(int socioId)
        {
            var cuotas = new List<Cuota>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, fecha_creacion 
                FROM cuota WHERE socio_id = @socio_id 
                ORDER BY fechaVencimiento DESC";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@socio_id", socioId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var cuota = new Cuota
                {
                    Id = reader.GetInt32("id"),
                    SocioId = reader.GetInt32("socio_id"),
                    Monto = reader.GetDouble("monto"),
                    FechaVencimiento = reader.GetDateTime("fechaVencimiento"),
                    Estado = Enum.Parse<EstadoCuota>(reader.GetString("estado")),
                    TipoCuota = Enum.Parse<TipoCuota>(reader.GetString("tipoDeCuota")),
                    MetodoPago = reader.IsDBNull("metodoPago") ? null : Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
                cuotas.Add(cuota);
            }

            return cuotas;
        }

        public async Task<List<Cuota>> ObtenerCuotasVencidasAsync()
        {
            var cuotas = new List<Cuota>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, fecha_creacion 
                FROM cuota 
                WHERE estado = 'Pendiente' AND fechaVencimiento < CURDATE()
                ORDER BY fechaVencimiento ASC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var cuota = new Cuota
                {
                    Id = reader.GetInt32("id"),
                    SocioId = reader.GetInt32("socio_id"),
                    Monto = reader.GetDouble("monto"),
                    FechaVencimiento = reader.GetDateTime("fechaVencimiento"),
                    Estado = Enum.Parse<EstadoCuota>(reader.GetString("estado")),
                    TipoCuota = Enum.Parse<TipoCuota>(reader.GetString("tipoDeCuota")),
                    MetodoPago = reader.IsDBNull("metodoPago") ? null : Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
                cuotas.Add(cuota);
            }

            return cuotas;
        }

        public async Task<List<Cuota>> ObtenerCuotasPendientesAsync()
        {
            var cuotas = new List<Cuota>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, fecha_creacion 
                FROM cuota 
                WHERE estado = 'Pendiente' AND fechaVencimiento >= CURDATE()
                ORDER BY fechaVencimiento ASC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var cuota = new Cuota
                {
                    Id = reader.GetInt32("id"),
                    SocioId = reader.GetInt32("socio_id"),
                    Monto = reader.GetDouble("monto"),
                    FechaVencimiento = reader.GetDateTime("fechaVencimiento"),
                    Estado = Enum.Parse<EstadoCuota>(reader.GetString("estado")),
                    TipoCuota = Enum.Parse<TipoCuota>(reader.GetString("tipoDeCuota")),
                    MetodoPago = reader.IsDBNull("metodoPago") ? null : Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
                cuotas.Add(cuota);
            }

            return cuotas;
        }

        public async Task<bool> TieneCuotasVencidasAsync(int socioId)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
        SELECT COUNT(1) FROM cuota 
        WHERE socio_id = @socio_id AND estado = 'Pendiente' AND fechaVencimiento < CURDATE()";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@socio_id", socioId);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<bool> ActualizarCuotaAsync(Cuota cuota)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                UPDATE cuota 
                SET monto = @monto, fechaVencimiento = @fechaVencimiento, estado = @estado, 
                    tipoDeCuota = @tipoDeCuota, metodoPago = @metodoPago
                WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@monto", cuota.Monto);
            cmd.Parameters.AddWithValue("@fechaVencimiento", cuota.FechaVencimiento);
            cmd.Parameters.AddWithValue("@estado", cuota.Estado.ToString());
            cmd.Parameters.AddWithValue("@tipoDeCuota", cuota.TipoCuota.ToString());
            cmd.Parameters.AddWithValue("@metodoPago", cuota.MetodoPago?.ToString() ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", cuota.Id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> MarcarCuotaComoPagadaAsync(int cuotaId, MetodoPago metodoPago)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            using var transaction = await conn.BeginTransactionAsync(); // ← TRANSACCIÓN

            try
            {
                // --- 1. OBTENER DATOS DE LA CUOTA ---
                const string getCuotaQuery = @"
            SELECT socio_id, monto FROM cuota WHERE id = @id";

                using var cmdGet = new MySqlCommand(getCuotaQuery, conn, transaction);
                cmdGet.Parameters.AddWithValue("@id", cuotaId);

                using var reader = await cmdGet.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                {
                    throw new Exception("Cuota no encontrada");
                }

                int socioId = reader.GetInt32("socio_id");
                double monto = reader.GetDouble("monto");
                await reader.CloseAsync();

                // --- 2. ACTUALIZAR CUOTA ---
                const string updateCuotaQuery = @"
            UPDATE cuota 
            SET estado = 'Pagada', metodoPago = @metodoPago 
            WHERE id = @id";

                using var cmdCuota = new MySqlCommand(updateCuotaQuery, conn, transaction);
                cmdCuota.Parameters.AddWithValue("@metodoPago", metodoPago.ToString());
                cmdCuota.Parameters.AddWithValue("@id", cuotaId);
                await cmdCuota.ExecuteNonQueryAsync();

                // --- 3. CREAR REGISTRO EN PAGO ---
                const string insertPagoQuery = @"
            INSERT INTO pago 
            (fechaPago, monto, metodoPago, cuota_id, socio_id, numero_comprobante) 
            VALUES (@fechaPago, @monto, @metodoPago, @cuota_id, @socio_id, @numero_comprobante)";

                using var cmdPago = new MySqlCommand(insertPagoQuery, conn, transaction);
                cmdPago.Parameters.AddWithValue("@fechaPago", DateTime.Now);
                cmdPago.Parameters.AddWithValue("@monto", monto);
                cmdPago.Parameters.AddWithValue("@metodoPago", metodoPago.ToString());
                cmdPago.Parameters.AddWithValue("@cuota_id", cuotaId);
                cmdPago.Parameters.AddWithValue("@socio_id", socioId);
                cmdPago.Parameters.AddWithValue("@numero_comprobante", $"PAGO-CUOTA-{cuotaId}-{DateTime.Now:yyyyMMddHHmmss}");

                await cmdPago.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> ActualizarEstadosCuotasVencidasAsync()
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                UPDATE cuota 
                SET estado = 'Vencida' 
                WHERE estado = 'Pendiente' AND fechaVencimiento < CURDATE()";

            using var cmd = new MySqlCommand(query, conn);
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> GenerarCuotasMensualesAsync(DateTime mesAnio, double monto)
        {
            int cuotasGeneradas = 0;

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            // Obtener socios activos
            const string querySocios = @"
                SELECT nroSocio FROM socio 
                WHERE habilitado = TRUE AND estado_pago = 'Completado'";

            var socios = new List<int>();
            using var cmdSocios = new MySqlCommand(querySocios, conn);
            using var reader = await cmdSocios.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                socios.Add(reader.GetInt32("nroSocio"));
            }
            await reader.CloseAsync();

            // Fecha de vencimiento (día 10 del mes)
            var fechaVencimiento = new DateTime(mesAnio.Year, mesAnio.Month, 10);

            foreach (var nroSocio in socios)
            {
                // Verificar si ya existe cuota para este mes
                const string checkQuery = @"
                    SELECT COUNT(1) FROM cuota 
                    WHERE socio_id = @socio_id 
                    AND YEAR(fechaVencimiento) = @anio 
                    AND MONTH(fechaVencimiento) = @mes";

                using var cmdCheck = new MySqlCommand(checkQuery, conn);
                cmdCheck.Parameters.AddWithValue("@socio_id", nroSocio);
                cmdCheck.Parameters.AddWithValue("@anio", mesAnio.Year);
                cmdCheck.Parameters.AddWithValue("@mes", mesAnio.Month);

                var existe = Convert.ToInt32(await cmdCheck.ExecuteScalarAsync()) > 0;

                if (!existe)
                {
                    var cuota = new Cuota(nroSocio, monto, fechaVencimiento, TipoCuota.Mensual);
                    await CrearCuotaAsync(cuota);
                    cuotasGeneradas++;
                }
            }

            return cuotasGeneradas;
        }

        public async Task<bool> InsertarCuotaAsync(Cuota cuota)
        {
            return await CrearCuotaAsync(cuota) > 0;
        }

        public async Task<int> ActualizarEstadoCuotasVencidasAsync()
        {
            return await ActualizarEstadosCuotasVencidasAsync();
        }

        public async Task<List<Cuota>> ObtenerCuotasQueVencenHoyAsync()
        {
            var cuotas = new List<Cuota>();
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
        SELECT id, socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, fecha_creacion 
        FROM cuota 
        WHERE estado = 'Pendiente' AND fechaVencimiento = CURDATE()
        ORDER BY fechaVencimiento ASC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var cuota = new Cuota
                {
                    Id = reader.GetInt32("id"),
                    SocioId = reader.GetInt32("socio_id"),
                    Monto = reader.GetDouble("monto"),
                    FechaVencimiento = reader.GetDateTime("fechaVencimiento"),
                    Estado = Enum.Parse<EstadoCuota>(reader.GetString("estado")),
                    TipoCuota = Enum.Parse<TipoCuota>(reader.GetString("tipoDeCuota")),
                    MetodoPago = reader.IsDBNull("metodoPago") ? null : Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
                cuotas.Add(cuota);
            }
            return cuotas;
        }

        public async Task<List<Cuota>> ObtenerCuotasImpagasAsync()
        {
            var cuotas = new List<Cuota>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
        SELECT id, socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, fecha_creacion
        FROM cuota
        WHERE estado IN ('Pendiente', 'Vencida')
        ORDER BY fechaVencimiento ASC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var cuota = new Cuota
                {
                    Id = reader.GetInt32("id"),
                    SocioId = reader.GetInt32("socio_id"),
                    Monto = reader.GetDouble("monto"),
                    FechaVencimiento = reader.GetDateTime("fechaVencimiento"),
                    Estado = Enum.Parse<EstadoCuota>(reader.GetString("estado")),
                    TipoCuota = Enum.Parse<TipoCuota>(reader.GetString("tipoDeCuota")),
                    MetodoPago = reader.IsDBNull("metodoPago") ? null : Enum.Parse<MetodoPago>(reader.GetString("metodoPago")),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };

                cuotas.Add(cuota);
            }

            return cuotas;
        }

    }
}
