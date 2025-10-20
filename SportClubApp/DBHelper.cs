using System;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Data;

namespace SportClubApp
{
    public static class DBHelper
    {
        private static string Conn => Config.ConnectionString;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(Conn);
        }

        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ============================================
        // MÉTODOS PARA PERSONA
        // ============================================
        public static bool PersonaExistsByDni(string dni)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT COUNT(1) FROM persona WHERE dni = @dni", conn);
            cmd.Parameters.AddWithValue("@dni", dni);
            var cnt = Convert.ToInt32(cmd.ExecuteScalar());
            return cnt > 0;
        }

        public static int InsertPersona(string nombre, string apellido, string dni, string telefono, string email, string tipoPersona)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"INSERT INTO persona (nombre, apellido, dni, telefono, email, tipo_persona) 
                  VALUES (@n, @a, @dni, @tel, @email, @tipo);
                  SELECT LAST_INSERT_ID();", conn);
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@a", apellido);
            cmd.Parameters.AddWithValue("@dni", dni);
            cmd.Parameters.AddWithValue("@tel", telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tipo", tipoPersona);
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        // ============================================
        // MÉTODOS PARA SOCIO
        // ============================================
        public static void InsertSocio(int personaId, DateTime fechaAlta, bool habilitado, bool aptoFisico, string carnet)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"INSERT INTO socio (persona_id, fechaAlta, habilitado, aptofisico, carnet, estado_pago)
                  VALUES (@pid, @fecha, @hab, @apto, @carnet, 'Pendiente')", conn);
            cmd.Parameters.AddWithValue("@pid", personaId);
            cmd.Parameters.AddWithValue("@fecha", fechaAlta);
            cmd.Parameters.AddWithValue("@hab", habilitado);
            cmd.Parameters.AddWithValue("@apto", aptoFisico);
            cmd.Parameters.AddWithValue("@carnet", carnet ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        // ============================================
        // MÉTODOS PARA NO SOCIO
        // ============================================
        public static void InsertNoSocio(int personaId, bool habilitado, DateTime? fechaVisita)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"INSERT INTO nosocio (persona_id, habilitado, fechaVisita)
                  VALUES (@pid, @hab, @fechaVisita)", conn);
            cmd.Parameters.AddWithValue("@pid", personaId);
            cmd.Parameters.AddWithValue("@hab", habilitado);
            cmd.Parameters.AddWithValue("@fechaVisita", fechaVisita.HasValue ? (object)fechaVisita.Value : DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        // ============================================
        // MÉTODOS PARA USUARIO
        // ============================================
        public static bool UsernameExists(string username)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT COUNT(1) FROM usuario WHERE username = @user", conn);
            cmd.Parameters.AddWithValue("@user", username);
            var cnt = Convert.ToInt32(cmd.ExecuteScalar());
            return cnt > 0;
        }

        public static void InsertUsuario(string username, string passwordPlain, string rol)
        {
            using var conn = GetConnection();
            conn.Open();
            var hashed = BCrypt.Net.BCrypt.HashPassword(passwordPlain);
            using var cmd = new MySqlCommand(
                @"INSERT INTO usuario (username, password, rol, activo) 
                  VALUES (@user, @pass, @rol, @activo)", conn);
            cmd.Parameters.AddWithValue("@user", username);
            cmd.Parameters.AddWithValue("@pass", hashed);
            cmd.Parameters.AddWithValue("@rol", rol);
            cmd.Parameters.AddWithValue("@activo", true);
            cmd.ExecuteNonQuery();
        }

        public static void InsertUsuarioConPersona(string username, string passwordPlain, string rol, int personaId)
        {
            using var conn = GetConnection();
            conn.Open();
            var hashed = BCrypt.Net.BCrypt.HashPassword(passwordPlain);
            using var cmd = new MySqlCommand(
                @"INSERT INTO usuario (username, password, rol, activo, persona_id) 
                  VALUES (@user, @pass, @rol, @activo, @pid)", conn);
            cmd.Parameters.AddWithValue("@user", username);
            cmd.Parameters.AddWithValue("@pass", hashed);
            cmd.Parameters.AddWithValue("@rol", rol);
            cmd.Parameters.AddWithValue("@activo", true);
            cmd.Parameters.AddWithValue("@pid", personaId);
            cmd.ExecuteNonQuery();
        }

        public static bool ValidateUser(string username, string passwordPlain)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT password FROM usuario WHERE username = @user AND activo = 1", conn);
            cmd.Parameters.AddWithValue("@user", username);
            var obj = cmd.ExecuteScalar();
            if (obj == null) return false;
            var hashed = obj.ToString();
            return BCrypt.Net.BCrypt.Verify(passwordPlain, hashed);
        }

        public static string GetUserRol(string username)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT rol FROM usuario WHERE username = @user AND activo = 1", conn);
            cmd.Parameters.AddWithValue("@user", username);
            var obj = cmd.ExecuteScalar();
            return obj?.ToString();
        }

        // ============================================
        // MÉTODOS PARA STRIPE
        // ============================================

        /// <summary>
        /// Actualiza la sesión de Stripe para un socio
        /// </summary>
        public static void UpdateSocioStripeSession(int personaId, string sessionId)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand(
                    @"UPDATE socio 
                      SET stripe_session_id = @sessionId,
                          estado_pago = 'Pendiente'
                      WHERE persona_id = @personaId", conn);
                cmd.Parameters.AddWithValue("@sessionId", sessionId);
                cmd.Parameters.AddWithValue("@personaId", personaId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar sesión Stripe: {ex.Message}");
            }
        }

        /// <summary>
        /// Marca el pago como completado
        /// </summary>
        public static void MarkPaymentAsCompleted(int personaId, string paymentIntentId)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand(
                    @"UPDATE socio 
                      SET 
                          estado_pago = 'Completado',
                          stripe_payment_intent_id = @paymentIntentId,
                          fecha_pago = NOW(),
                          habilitado = TRUE
                      WHERE persona_id = @personaId", conn);
                cmd.Parameters.AddWithValue("@personaId", personaId);
                cmd.Parameters.AddWithValue("@paymentIntentId", paymentIntentId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al marcar pago como completado: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el estado de pago de un socio
        /// </summary>
        public static string GetPaymentStatus(int personaId)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand(
                    @"SELECT estado_pago 
                      FROM socio 
                      WHERE persona_id = @personaId", conn);
                cmd.Parameters.AddWithValue("@personaId", personaId);
                object result = cmd.ExecuteScalar();
                return result?.ToString() ?? "Pendiente";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estado de pago: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el sessionId de Stripe para un socio
        /// </summary>
        public static string GetSocioStripeSessionId(int personaId)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand(
                    @"SELECT stripe_session_id 
                      FROM socio 
                      WHERE persona_id = @personaId", conn);
                cmd.Parameters.AddWithValue("@personaId", personaId);
                object result = cmd.ExecuteScalar();
                return result?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener sessionId de Stripe: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene los detalles de pago de un socio
        /// </summary>
        public static (string estado, DateTime? fechaPago, string paymentIntentId) GetDetallesPago(int personaId)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new MySqlCommand(
                    @"SELECT estado_pago, fecha_pago, stripe_payment_intent_id
                      FROM socio 
                      WHERE persona_id = @personaId", conn);
                cmd.Parameters.AddWithValue("@personaId", personaId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string estado = reader["estado_pago"].ToString();
                    DateTime? fechaPago = reader["fecha_pago"] == DBNull.Value ? null : (DateTime?)reader["fecha_pago"];
                    string paymentIntentId = reader["stripe_payment_intent_id"] == DBNull.Value ? null : reader["stripe_payment_intent_id"].ToString();
                    return (estado, fechaPago, paymentIntentId);
                }
                return ("Pendiente", null, null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener detalles de pago: {ex.Message}");
            }
        }

        // ============================================
        // MÉTODOS PARA CRUD DE ADMINISTRADORES
        // ============================================

        /// <summary>
        /// Actualiza un administrador (username y/o password)
        /// </summary>
        public static bool ActualizarAdministrador(int id, string nuevoUsername, string nuevaPassword = null)
        {
            using var conn = GetConnection();
            conn.Open();

            if (!string.IsNullOrEmpty(nuevaPassword))
            {
                // Actualizar username Y password
                using var cmd = new MySqlCommand(
                    "UPDATE usuario SET username = @username, password = @password WHERE id = @id AND rol = 'Administrador'",
                    conn);
                cmd.Parameters.AddWithValue("@username", nuevoUsername);
                cmd.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(nuevaPassword));
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            else
            {
                // Actualizar solo username
                using var cmd = new MySqlCommand(
                    "UPDATE usuario SET username = @username WHERE id = @id AND rol = 'Administrador'",
                    conn);
                cmd.Parameters.AddWithValue("@username", nuevoUsername);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Obtiene todos los administradores para el CRUD
        /// </summary>
        public static DataTable GetAdministradores()
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"SELECT id, username, fecha_creacion, 
                         CASE WHEN activo = 1 THEN 'Activo' ELSE 'Inactivo' END as estado 
                   FROM usuario 
                   WHERE rol = 'Administrador' 
                   ORDER BY fecha_creacion DESC",
                conn);

            var dt = new DataTable();
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Elimina un administrador por ID
        /// </summary>
        public static bool EliminarAdministrador(int id)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                "DELETE FROM usuario WHERE id = @id AND rol = 'Administrador'",
                conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Verifica si un username existe (excluyendo un ID específico para edición)
        /// </summary>
        public static bool UsernameExists(string username, int excludeId = 0)
        {
            using var conn = GetConnection();
            conn.Open();

            string query;
            if (excludeId > 0)
            {
                query = "SELECT COUNT(1) FROM usuario WHERE username = @user AND id != @excludeId";
            }
            else
            {
                query = "SELECT COUNT(1) FROM usuario WHERE username = @user";
            }

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@user", username);
            if (excludeId > 0)
            {
                cmd.Parameters.AddWithValue("@excludeId", excludeId);
            }

            var cnt = Convert.ToInt32(cmd.ExecuteScalar());
            return cnt > 0;
        }
    }
}