using MySql.Data.MySqlClient;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;
using System.Data;

namespace SportClubApp.Data.Repositories
{
    public class NoSocioRepository : INoSocioRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public NoSocioRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> InsertarAsync(NoSocio noSocio)
        {
            try
            {
                using var connection = _dbConnection.GetConnection();
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO nosocio (
                        persona_id, 
                        habilitado, 
                        fechaVisita,
                        foto_visita,
                        foto_tipo,
                        stripe_session_id,
                        stripe_payment_intent_id,
                        estado_pago,
                        fecha_pago,
                        monto_pago
                    )
                    VALUES (
                        @personaId, 
                        @habilitado, 
                        @fechaVisita,
                        @foto_visita,
                        @foto_tipo,
                        @stripe_session_id,
                        @stripe_payment_intent_id,
                        @estado_pago,
                        @fecha_pago,
                        @monto_pago
                    );
                    SELECT LAST_INSERT_ID();";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@personaId", noSocio.Id);
                cmd.Parameters.AddWithValue("@habilitado", noSocio.Habilitado);
                cmd.Parameters.AddWithValue("@fechaVisita", noSocio.FechaVisita);

                // ✅ FOTO PARA PASE DIARIO
                if (noSocio.FotoVisita != null && noSocio.FotoVisita.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@foto_visita", noSocio.FotoVisita);
                    cmd.Parameters.AddWithValue("@foto_tipo", "image/jpeg");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@foto_visita", DBNull.Value);
                    cmd.Parameters.AddWithValue("@foto_tipo", DBNull.Value);
                }

                // ✅ STRIPE
                cmd.Parameters.AddWithValue("@stripe_session_id", noSocio.StripeSessionId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@stripe_payment_intent_id", noSocio.StripePaymentIntentId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@estado_pago", noSocio.EstadoPago.ToString());
                cmd.Parameters.AddWithValue("@fecha_pago", noSocio.FechaPago ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@monto_pago", noSocio.MontoPago);

                var idNoSocio = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                noSocio.IdNoSocio = idNoSocio;

                return idNoSocio;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar NoSocio en la base de datos.", ex);
            }
        }

        public async Task<NoSocio> ObtenerPorPersonaIdAsync(int personaId)
        {
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            const string query = @"
                SELECT 
                    ns.id,
                    ns.persona_id,
                    ns.habilitado,
                    ns.fechaVisita,
                    ns.foto_visita,
                    ns.foto_tipo,
                    ns.stripe_session_id,
                    ns.stripe_payment_intent_id,
                    ns.estado_pago,
                    ns.fecha_pago,
                    ns.monto_pago,
                    p.nombre,
                    p.apellido,
                    p.dni,
                    p.telefono,
                    p.email,
                    p.fecha_registro
                FROM nosocio ns
                INNER JOIN persona p ON ns.persona_id = p.id
                WHERE ns.persona_id = @personaId";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@personaId", personaId);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var noSocio = new NoSocio(
                    reader.GetString("nombre"),
                    reader.GetString("apellido"),
                    reader.GetString("dni"),
                    reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                    reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email")
                )
                {
                    Id = reader.GetInt32("persona_id"),
                    IdNoSocio = reader.GetInt32("id"),
                    Habilitado = reader.GetBoolean("habilitado"),
                    FechaVisita = reader.IsDBNull(reader.GetOrdinal("fechaVisita")) ? null : reader.GetDateTime("fechaVisita"),
                    FotoVisita = reader.IsDBNull(reader.GetOrdinal("foto_visita")) ? null : (byte[])reader["foto_visita"],
                    StripeSessionId = reader.IsDBNull(reader.GetOrdinal("stripe_session_id")) ? null : reader.GetString("stripe_session_id"),
                    StripePaymentIntentId = reader.IsDBNull(reader.GetOrdinal("stripe_payment_intent_id")) ? null : reader.GetString("stripe_payment_intent_id"),
                    EstadoPago = Enum.Parse<EstadoPago>(reader.GetString("estado_pago")),
                    FechaPago = reader.IsDBNull(reader.GetOrdinal("fecha_pago")) ? null : reader.GetDateTime("fecha_pago"),
                    MontoPago = reader.GetDouble("monto_pago"),
                    FechaRegistro = reader.GetDateTime("fecha_registro")
                };

                return noSocio;
            }

            return null;
        }

        public async Task<NoSocio> ObtenerNoSocioPorIdAsync(int idNoSocio)
        {
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            const string query = @"
                SELECT 
                    ns.id,
                    ns.persona_id,
                    ns.habilitado,
                    ns.fechaVisita,
                    ns.foto_visita,
                    ns.foto_tipo,
                    ns.stripe_session_id,
                    ns.stripe_payment_intent_id,
                    ns.estado_pago,
                    ns.fecha_pago,
                    ns.monto_pago,
                    p.nombre,
                    p.apellido,
                    p.dni,
                    p.telefono,
                    p.email,
                    p.fecha_registro
                FROM nosocio ns
                INNER JOIN persona p ON ns.persona_id = p.id
                WHERE ns.id = @idNoSocio";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@idNoSocio", idNoSocio);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var noSocio = new NoSocio(
                    reader.GetString("nombre"),
                    reader.GetString("apellido"),
                    reader.GetString("dni"),
                    reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
                    reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email")
                )
                {
                    Id = reader.GetInt32("persona_id"),
                    IdNoSocio = reader.GetInt32("id"),
                    Habilitado = reader.GetBoolean("habilitado"),
                    FechaVisita = reader.IsDBNull(reader.GetOrdinal("fechaVisita")) ? null : reader.GetDateTime("fechaVisita"),
                    FotoVisita = reader.IsDBNull(reader.GetOrdinal("foto_visita")) ? null : (byte[])reader["foto_visita"],
                    StripeSessionId = reader.IsDBNull(reader.GetOrdinal("stripe_session_id")) ? null : reader.GetString("stripe_session_id"),
                    StripePaymentIntentId = reader.IsDBNull(reader.GetOrdinal("stripe_payment_intent_id")) ? null : reader.GetString("stripe_payment_intent_id"),
                    EstadoPago = Enum.Parse<EstadoPago>(reader.GetString("estado_pago")),
                    FechaPago = reader.IsDBNull(reader.GetOrdinal("fecha_pago")) ? null : reader.GetDateTime("fecha_pago"),
                    MontoPago = reader.GetDouble("monto_pago"),
                    FechaRegistro = reader.GetDateTime("fecha_registro")
                };

                return noSocio;
            }

            return null;
        }

        public async Task<bool> ExistePorPersonaIdAsync(int personaId)
        {
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            const string query = "SELECT COUNT(1) FROM nosocio WHERE persona_id = @personaId";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@personaId", personaId);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<bool> MarcarPagoCompletadoAsync(int idNoSocio, string paymentIntentId)
        {
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            const string query = @"
                UPDATE nosocio 
                SET 
                    estado_pago = 'Completado',
                    stripe_payment_intent_id = @paymentIntentId,
                    fecha_pago = @fecha_pago,
                    habilitado = TRUE
                WHERE id = @idNoSocio";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@paymentIntentId", paymentIntentId);
            cmd.Parameters.AddWithValue("@fecha_pago", DateTime.Now);
            cmd.Parameters.AddWithValue("@idNoSocio", idNoSocio);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ActualizarNoSocioAsync(NoSocio noSocio)
        {
            using var connection = _dbConnection.GetConnection();
            await connection.OpenAsync();

            const string query = @"
                UPDATE nosocio 
                SET 
                    stripe_session_id = @stripeSessionId,
                    estado_pago = @estadoPago,
                    monto_pago = @montoPago
                WHERE id = @idNoSocio";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@stripeSessionId", noSocio.StripeSessionId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@estadoPago", noSocio.EstadoPago.ToString());
            cmd.Parameters.AddWithValue("@montoPago", noSocio.MontoPago);
            cmd.Parameters.AddWithValue("@idNoSocio", noSocio.IdNoSocio);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}