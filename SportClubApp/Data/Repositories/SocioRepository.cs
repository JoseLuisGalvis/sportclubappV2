// Data/Repositories/SocioRepository.cs
using MySql.Data.MySqlClient;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;
using System.Data;

using Dapper;
using SportClubApp.Services;


namespace SportClubApp.Data.Repositories
{
    public class SocioRepository : ISocioRepository
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly IPersonaRepository _personaRepository;

        public SocioRepository(IDatabaseConnection dbConnection, IPersonaRepository personaRepository)
        {
            _dbConnection = dbConnection;
            _personaRepository = personaRepository;
        }

        // ================================================================
        // CREAR SOCIO (CON TODOS LOS CAMPOS DE TU TABLA)
        // ================================================================

        public async Task<int> CrearSocioAsync(Socio socio)
        {
            if (!socio.Validar(out string mensajeError))
                throw new ArgumentException(mensajeError);

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                INSERT INTO socio (
                    persona_id, 
                    fechaAlta, 
                    habilitado, 
                    aptofisico, 
                    foto_carnet, 
                    foto_tipo,
                    carnet, 
                    fecha_emision_carnet,
                    stripe_session_id, 
                    stripe_payment_intent_id, 
                    estado_pago, 
                    fecha_pago, 
                    monto_pago
                ) 
                VALUES (
                    @persona_id, 
                    @fechaAlta, 
                    @habilitado, 
                    @aptofisico, 
                    @foto_carnet, 
                    @foto_tipo,
                    @carnet, 
                    @fecha_emision_carnet,
                    @stripe_session_id, 
                    @stripe_payment_intent_id, 
                    @estado_pago, 
                    @fecha_pago, 
                    @monto_pago
                );
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@persona_id", socio.Id);
            cmd.Parameters.AddWithValue("@fechaAlta", socio.FechaAlta);
            cmd.Parameters.AddWithValue("@habilitado", socio.Habilitado);
            cmd.Parameters.AddWithValue("@aptofisico", socio.AptoFisico);

            // ✅ FOTO DEL CARNET
            if (socio.FotoCarnet != null && socio.FotoCarnet.Length > 0)
            {
                cmd.Parameters.AddWithValue("@foto_carnet", socio.FotoCarnet);
                cmd.Parameters.AddWithValue("@foto_tipo", "image/jpeg"); // Asumimos JPEG
            }
            else
            {
                cmd.Parameters.AddWithValue("@foto_carnet", DBNull.Value);
                cmd.Parameters.AddWithValue("@foto_tipo", DBNull.Value);
            }

            // ✅ CARNET
            cmd.Parameters.AddWithValue("@carnet", socio.Carnet ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fecha_emision_carnet",
                string.IsNullOrEmpty(socio.Carnet) ? (object)DBNull.Value : DateTime.Now);

            // ✅ STRIPE
            cmd.Parameters.AddWithValue("@stripe_session_id", socio.StripeSessionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@stripe_payment_intent_id", socio.StripePaymentIntentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@estado_pago", socio.EstadoPago.ToString());
            cmd.Parameters.AddWithValue("@fecha_pago", socio.FechaPago ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@monto_pago", socio.MontoPago);

            var nroSocio = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            socio.NroSocio = nroSocio;

            // Generar carnet si no existe
            if (string.IsNullOrEmpty(socio.Carnet))
            {
                socio.GenerarCarnet();
                await ActualizarCarnetAsync(nroSocio, socio.Carnet);
            }

            return nroSocio;
        }

        // ================================================================
        // OBTENER SOCIO POR ID
        // ================================================================

        public async Task<Socio> ObtenerSocioPorIdAsync(int id)
        {
            return await ObtenerSocioPorNroAsync(id);
        }

        // ================================================================
        // OBTENER SOCIO POR NÚMERO (INCLUYE TODOS LOS CAMPOS)
        // ================================================================

        public async Task<Socio> ObtenerSocioPorNroAsync(int nroSocio)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT 
                    s.nroSocio, 
                    s.persona_id, 
                    s.fechaAlta, 
                    s.habilitado, 
                    s.aptofisico, 
                    s.foto_carnet,
                    s.foto_tipo,
                    s.carnet,
                    s.fecha_emision_carnet,
                    s.stripe_session_id, 
                    s.stripe_payment_intent_id, 
                    s.estado_pago, 
                    s.fecha_pago, 
                    s.monto_pago,
                    p.nombre, 
                    p.apellido, 
                    p.dni, 
                    p.telefono, 
                    p.email, 
                    p.fecha_registro, 
                    p.tipo_persona
                FROM socio s
                INNER JOIN persona p ON s.persona_id = p.id
                WHERE s.nroSocio = @nroSocio";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var socio = new Socio(
                    reader.GetString("nombre"),
                    reader.GetString("apellido"),
                    reader.GetString("dni"),
                    reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                    reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email")
                )
                {
                    Id = reader.GetInt32("persona_id"),
                    NroSocio = reader.GetInt32("nroSocio"),
                    FechaAlta = reader.GetDateTime("fechaAlta"),
                    Habilitado = reader.GetBoolean("habilitado"),
                    AptoFisico = reader.GetBoolean("aptofisico"),

                    // ✅ FOTO DEL CARNET
                    FotoCarnet = reader.IsDBNull(reader.GetOrdinal("foto_carnet"))
                        ? null
                        : (byte[])reader["foto_carnet"],

                    // ✅ CARNET
                    Carnet = reader.IsDBNull(reader.GetOrdinal("carnet"))
                        ? null
                        : reader.GetString("carnet"),

                    // ✅ STRIPE
                    StripeSessionId = reader.IsDBNull(reader.GetOrdinal("stripe_session_id"))
                        ? null
                        : reader.GetString("stripe_session_id"),

                    StripePaymentIntentId = reader.IsDBNull(reader.GetOrdinal("stripe_payment_intent_id"))
                        ? null
                        : reader.GetString("stripe_payment_intent_id"),

                    EstadoPago = Enum.Parse<EstadoPago>(reader.GetString("estado_pago")),

                    FechaPago = reader.IsDBNull(reader.GetOrdinal("fecha_pago"))
                        ? null
                        : reader.GetDateTime("fecha_pago"),

                    MontoPago = reader.GetDouble("monto_pago"),
                    FechaRegistro = reader.GetDateTime("fecha_registro")
                };

                return socio;
            }

            return null;
        }

        // ================================================================
        // OBTENER TODOS LOS SOCIOS
        // ================================================================

        public async Task<List<Socio>> ObtenerTodosLosSociosAsync()
        {
            var socios = new List<Socio>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT 
                    s.nroSocio, 
                    s.persona_id, 
                    s.fechaAlta, 
                    s.habilitado, 
                    s.aptofisico, 
                    s.foto_carnet,
                    s.foto_tipo,
                    s.carnet,
                    s.fecha_emision_carnet,
                    s.stripe_session_id, 
                    s.stripe_payment_intent_id, 
                    s.estado_pago, 
                    s.fecha_pago, 
                    s.monto_pago,
                    p.nombre, 
                    p.apellido, 
                    p.dni, 
                    p.telefono, 
                    p.email, 
                    p.fecha_registro, 
                    p.tipo_persona
                FROM socio s
                INNER JOIN persona p ON s.persona_id = p.id
                ORDER BY s.nroSocio";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var socio = new Socio(
                    reader.GetString("nombre"),
                    reader.GetString("apellido"),
                    reader.GetString("dni"),
                    reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                    reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email")
                )
                {
                    Id = reader.GetInt32("persona_id"),
                    NroSocio = reader.GetInt32("nroSocio"),
                    FechaAlta = reader.GetDateTime("fechaAlta"),
                    Habilitado = reader.GetBoolean("habilitado"),
                    AptoFisico = reader.GetBoolean("aptofisico"),

                    // ✅ FOTO
                    FotoCarnet = reader.IsDBNull(reader.GetOrdinal("foto_carnet"))
                        ? null
                        : (byte[])reader["foto_carnet"],

                    // ✅ CARNET
                    Carnet = reader.IsDBNull(reader.GetOrdinal("carnet"))
                        ? null
                        : reader.GetString("carnet"),

                    // ✅ STRIPE
                    StripeSessionId = reader.IsDBNull(reader.GetOrdinal("stripe_session_id"))
                        ? null
                        : reader.GetString("stripe_session_id"),

                    StripePaymentIntentId = reader.IsDBNull(reader.GetOrdinal("stripe_payment_intent_id"))
                        ? null
                        : reader.GetString("stripe_payment_intent_id"),

                    EstadoPago = Enum.Parse<EstadoPago>(reader.GetString("estado_pago")),

                    FechaPago = reader.IsDBNull(reader.GetOrdinal("fecha_pago"))
                        ? null
                        : reader.GetDateTime("fecha_pago"),

                    MontoPago = reader.GetDouble("monto_pago"),
                    FechaRegistro = reader.GetDateTime("fecha_registro")
                };

                socios.Add(socio);
            }

            return socios;
        }

        // ================================================================
        // ACTUALIZAR SOCIO
        // ================================================================

        public async Task<bool> ActualizarSocioAsync(Socio socio)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                UPDATE socio 
                SET 
                    habilitado = @habilitado, 
                    aptofisico = @aptofisico, 
                    carnet = @carnet,
                    stripe_session_id = @stripe_session_id,
                    stripe_payment_intent_id = @stripe_payment_intent_id,
                    estado_pago = @estado_pago, 
                    fecha_pago = @fecha_pago, 
                    monto_pago = @monto_pago
                WHERE nroSocio = @nroSocio";

            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@habilitado", socio.Habilitado);
            cmd.Parameters.AddWithValue("@aptofisico", socio.AptoFisico);
            cmd.Parameters.AddWithValue("@carnet", socio.Carnet ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@stripe_session_id", socio.StripeSessionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@stripe_payment_intent_id", socio.StripePaymentIntentId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@estado_pago", socio.EstadoPago.ToString());
            cmd.Parameters.AddWithValue("@fecha_pago", socio.FechaPago ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@monto_pago", socio.MontoPago);
            cmd.Parameters.AddWithValue("@nroSocio", socio.NroSocio);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ================================================================
        // HABILITAR SOCIO
        // ================================================================

        public async Task<bool> HabilitarSocioAsync(int nroSocio)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "UPDATE socio SET habilitado = TRUE WHERE nroSocio = @nroSocio";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ================================================================
        // DESHABILITAR SOCIO
        // ================================================================

        public async Task<bool> DeshabilitarSocioAsync(int nroSocio)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "UPDATE socio SET habilitado = FALSE WHERE nroSocio = @nroSocio";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ================================================================
        // MARCAR PAGO COMPLETADO
        // ================================================================

        public async Task<bool> MarcarPagoCompletadoAsync(int nroSocio, string paymentIntentId)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                // --- 1. ACTUALIZAR SOCIO ---
                const string updateSocioQuery = @"
            UPDATE socio 
            SET estado_pago = 'Completado', 
                stripe_payment_intent_id = @paymentIntentId,
                fecha_pago = @fecha_pago,
                habilitado = TRUE
            WHERE nroSocio = @nroSocio";

                using var cmdSocio = new MySqlCommand(updateSocioQuery, conn, transaction);
                cmdSocio.Parameters.AddWithValue("@paymentIntentId", paymentIntentId);
                cmdSocio.Parameters.AddWithValue("@fecha_pago", DateTime.Now);
                cmdSocio.Parameters.AddWithValue("@nroSocio", nroSocio);
                await cmdSocio.ExecuteNonQueryAsync();

                // --- 2. CREAR PRIMERA CUOTA ---
                const string insertCuotaQuery = @"
            INSERT INTO cuota 
            (socio_id, monto, fechaVencimiento, estado, tipoDeCuota, metodoPago, stripe_payment_intent_id) 
            VALUES (@socio_id, @monto, @fechaVencimiento, @estado, @tipoDeCuota, @metodoPago, @stripe_payment_intent_id);
            SELECT LAST_INSERT_ID();";

                using var cmdCuota = new MySqlCommand(insertCuotaQuery, conn, transaction);
                cmdCuota.Parameters.AddWithValue("@socio_id", nroSocio);
                cmdCuota.Parameters.AddWithValue("@monto", 100000.00);
                cmdCuota.Parameters.AddWithValue("@fechaVencimiento", DateTime.Now.AddMonths(1));
                cmdCuota.Parameters.AddWithValue("@estado", "Pagada");
                cmdCuota.Parameters.AddWithValue("@tipoDeCuota", "Mensual");
                cmdCuota.Parameters.AddWithValue("@metodoPago", "Stripe");
                // ✅ ESTA ES LA LÍNEA QUE FALTA:
                cmdCuota.Parameters.AddWithValue("@stripe_payment_intent_id", paymentIntentId);

                var cuotaId = Convert.ToInt32(await cmdCuota.ExecuteScalarAsync());

                // --- 3. REGISTRAR PAGO ---
                const string insertPagoQuery = @"
            INSERT INTO pago 
            (fechaPago, monto, metodoPago, cuota_id, socio_id, numero_comprobante, stripe_payment_intent_id) 
            VALUES (@fechaPago, @monto, @metodoPago, @cuota_id, @socio_id, @numero_comprobante, @stripe_payment_intent_id)";

                using var cmdPago = new MySqlCommand(insertPagoQuery, conn, transaction);
                cmdPago.Parameters.AddWithValue("@fechaPago", DateTime.Now);
                cmdPago.Parameters.AddWithValue("@monto", 100000.00);
                cmdPago.Parameters.AddWithValue("@metodoPago", "Stripe");
                cmdPago.Parameters.AddWithValue("@cuota_id", cuotaId);
                cmdPago.Parameters.AddWithValue("@socio_id", nroSocio);
                cmdPago.Parameters.AddWithValue("@numero_comprobante", $"PAGO-INICIAL-{nroSocio}-{DateTime.Now:yyyyMMddHHmmss}");
                // ✅ ESTA ES LA LÍNEA QUE FALTA:
                cmdPago.Parameters.AddWithValue("@stripe_payment_intent_id", paymentIntentId);

                await cmdPago.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"ERROR en MarcarPagoCompletadoAsync: {ex.Message}");
                throw;
            }
        }

        // ================================================================
        // VERIFICAR SI EXISTE SOCIO POR PERSONA ID
        // ================================================================

        public async Task<bool> ExisteSocioPorPersonaIdAsync(int personaId)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "SELECT COUNT(1) FROM socio WHERE persona_id = @persona_id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@persona_id", personaId);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        // ================================================================
        // ACTUALIZAR CARNET (MÉTODO PRIVADO)
        // ================================================================

        private async Task<bool> ActualizarCarnetAsync(int nroSocio, string carnet)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                UPDATE socio 
                SET 
                    carnet = @carnet,
                    fecha_emision_carnet = @fecha_emision
                WHERE nroSocio = @nroSocio";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@carnet", carnet);
            cmd.Parameters.AddWithValue("@fecha_emision", DateTime.Now);
            cmd.Parameters.AddWithValue("@nroSocio", nroSocio);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ================================================================
        // OBTENER SOCIOS CON PAGO COMPLETADO
        // ================================================================
        public async Task<List<Socio>> ObtenerSociosConPagoCompletado()
        {
            var socios = new List<Socio>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
        SELECT 
            s.nroSocio, 
            s.persona_id, 
            s.fechaAlta, 
            s.habilitado, 
            s.aptofisico, 
            s.foto_carnet,
            s.foto_tipo,
            s.carnet,
            s.fecha_emision_carnet,
            s.stripe_session_id, 
            s.stripe_payment_intent_id, 
            s.estado_pago, 
            s.fecha_pago, 
            s.monto_pago,
            p.nombre, 
            p.apellido, 
            p.dni, 
            p.telefono, 
            p.email, 
            p.fecha_registro
        FROM socio s
        INNER JOIN persona p ON s.persona_id = p.id
        WHERE s.estado_pago = 'Completado'
        ORDER BY s.fecha_pago DESC, p.apellido, p.nombre";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var socio = new Socio(
                    reader.GetString("nombre"),
                    reader.GetString("apellido"),
                    reader.GetString("dni"),
                    reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                    reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email")
                )
                {
                    Id = reader.GetInt32("persona_id"),
                    NroSocio = reader.GetInt32("nroSocio"),
                    FechaAlta = reader.GetDateTime("fechaAlta"),
                    Habilitado = reader.GetBoolean("habilitado"),
                    AptoFisico = reader.GetBoolean("aptofisico"),
                    FotoCarnet = reader.IsDBNull(reader.GetOrdinal("foto_carnet")) ? null : (byte[])reader["foto_carnet"],
                    Carnet = reader.IsDBNull(reader.GetOrdinal("carnet")) ? null : reader.GetString("carnet"),
                    StripeSessionId = reader.IsDBNull(reader.GetOrdinal("stripe_session_id")) ? null : reader.GetString("stripe_session_id"),
                    StripePaymentIntentId = reader.IsDBNull(reader.GetOrdinal("stripe_payment_intent_id")) ? null : reader.GetString("stripe_payment_intent_id"),
                    EstadoPago = Enum.Parse<EstadoPago>(reader.GetString("estado_pago")),
                    FechaPago = reader.IsDBNull(reader.GetOrdinal("fecha_pago")) ? null : reader.GetDateTime("fecha_pago"),
                    MontoPago = reader.GetDouble("monto_pago"),
                    FechaRegistro = reader.GetDateTime("fecha_registro")
                };

                socios.Add(socio);
            }

            return socios;
        }


        public async Task<Services.SocioInfo> ObtenerSocioParaCarnetAsync(int nroSocio)
        {
            const string query = @"
        SELECT 
            s.nroSocio as NroSocio,
            CONCAT(p.nombre, ' ', p.apellido) as NombreCompleto,
            p.dni as DNI,
            s.estado_pago as EstadoPago,
            s.foto_carnet as FotoCarnet,
            s.carnet as CarnetCodigo
        FROM socio s
        INNER JOIN persona p ON s.persona_id = p.id
        WHERE s.nroSocio = @NroSocio AND s.estado_pago = 'Completado'";

            try
            {
                using var connection = _dbConnection.GetConnection();
                await connection.OpenAsync();

                var socio = await connection.QueryFirstOrDefaultAsync<Services.SocioInfo>(query, new { NroSocio = nroSocio });
                return socio;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos para carnet: {ex.Message}", ex);
            }
        }

        public async Task<bool> MarcarCarnetEntregadoAsync(int nroSocio, string codigoCarnet)
        {
            const string query = @"
                UPDATE socio 
                SET fecha_emision_carnet = @FechaEmision,
                    carnet = @CarnetCodigo,
                    carnet_entregado = 1,
                    fecha_entrega_carnet = NOW()
                WHERE nroSocio = @NroSocio";

            try
            {
                using var connection = _dbConnection.GetConnection();
                await connection.OpenAsync();

                var parametros = new
                {
                    NroSocio = nroSocio,
                    CarnetCodigo = codigoCarnet ?? $"SCG5-{nroSocio:000000}",
                    FechaEmision = DateTime.Now
                };

                int affectedRows = await connection.ExecuteAsync(query, parametros);
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al marcar carnet como entregado: {ex.Message}", ex);
            }
        }


    }

}