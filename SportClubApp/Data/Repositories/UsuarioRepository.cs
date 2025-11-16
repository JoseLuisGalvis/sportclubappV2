// Data/Repositories/UsuarioRepository.cs
using MySql.Data.MySqlClient;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;
using System.Data;
using System.Text;

namespace SportClubApp.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public UsuarioRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CrearUsuarioAsync(Usuario usuario, string password)
        {
            if (!usuario.Validar(out string mensajeError))
                throw new ArgumentException(mensajeError);

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            const string query = @"
                INSERT INTO usuario (username, password, rol, activo, persona_id, fecha_creacion) 
                VALUES (@username, @password, @rol, @activo, @persona_id, @fecha_creacion);
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", usuario.Username);
            cmd.Parameters.AddWithValue("@password", passwordHash);
            cmd.Parameters.AddWithValue("@rol", usuario.Rol.ToString());
            cmd.Parameters.AddWithValue("@activo", usuario.Activo);
            cmd.Parameters.AddWithValue("@persona_id", usuario.PersonaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@fecha_creacion", usuario.FechaCreacion);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            usuario.Id = id;
            return id;
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, username, password, rol, activo, persona_id, fecha_creacion 
                FROM usuario WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = reader.GetInt32("id"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Rol = Enum.Parse<Rol>(reader.GetString("rol")),
                    Activo = reader.GetBoolean("activo"),
                    PersonaId = reader.IsDBNull("persona_id") ? null : reader.GetInt32("persona_id"),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
            }
            return null;
        }

        public async Task<Usuario> ObtenerUsuarioPorUsernameAsync(string username)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, username, password, rol, activo, persona_id, fecha_creacion 
                FROM usuario WHERE username = @username";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    Id = reader.GetInt32("id"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Rol = Enum.Parse<Rol>(reader.GetString("rol")),
                    Activo = reader.GetBoolean("activo"),
                    PersonaId = reader.IsDBNull("persona_id") ? null : reader.GetInt32("persona_id"),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
            }
            return null;
        }

        public async Task<bool> ValidarCredencialesAsync(string username, string password)
        {
            var usuario = await ObtenerUsuarioPorUsernameAsync(username);
            if (usuario == null || !usuario.Activo)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, usuario.Password);
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                UPDATE usuario 
                SET username = @username, rol = @rol, activo = @activo, persona_id = @persona_id
                WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", usuario.Username);
            cmd.Parameters.AddWithValue("@rol", usuario.Rol.ToString());
            cmd.Parameters.AddWithValue("@activo", usuario.Activo);
            cmd.Parameters.AddWithValue("@persona_id", usuario.PersonaId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", usuario.Id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> CambiarPasswordAsync(int usuarioId, string nuevaPassword)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "UPDATE usuario SET password = @password WHERE id = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@password", passwordHash);
            cmd.Parameters.AddWithValue("@id", usuarioId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> ExisteUsernameAsync(string username, int? excludeId = null)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            string query = "SELECT COUNT(1) FROM usuario WHERE username = @username";
            if (excludeId.HasValue)
                query += " AND id != @excludeId";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            if (excludeId.HasValue)
                cmd.Parameters.AddWithValue("@excludeId", excludeId.Value);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<List<Usuario>> ObtenerTodosLosUsuariosAsync()
        {
            var usuarios = new List<Usuario>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "SELECT id, username, password, rol, activo, persona_id, fecha_creacion FROM usuario";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var usuario = new Usuario
                {
                    Id = reader.GetInt32("id"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Rol = Enum.Parse<Rol>(reader.GetString("rol")),
                    Activo = reader.GetBoolean("activo"),
                    PersonaId = reader.IsDBNull("persona_id") ? null : reader.GetInt32("persona_id"),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public async Task<bool> ActivarUsuarioAsync(int usuarioId)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "UPDATE usuario SET activo = TRUE WHERE id = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", usuarioId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DesactivarUsuarioAsync(int usuarioId)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "UPDATE usuario SET activo = FALSE WHERE id = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", usuarioId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ✅ MÉTODOS NUEVOS PARA ADMINISTRADORES
        public async Task<List<Usuario>> GetAdministradoresAsync()
        {
            var usuarios = new List<Usuario>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, username, password, rol, activo, persona_id, fecha_creacion 
                FROM usuario 
                WHERE rol = 'Administrador' 
                ORDER BY fecha_creacion DESC";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var usuario = new Usuario
                {
                    Id = reader.GetInt32("id"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Rol = Enum.Parse<Rol>(reader.GetString("rol")),
                    Activo = reader.GetBoolean("activo"),
                    PersonaId = reader.IsDBNull("persona_id") ? null : reader.GetInt32("persona_id"),
                    FechaCreacion = reader.GetDateTime("fecha_creacion")
                };
                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public async Task<bool> EliminarAdministradorAsync(int id)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "DELETE FROM usuario WHERE id = @id AND rol = 'Administrador'";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // ✅ MÉTODO ADICIONAL: Actualizar administrador
        public async Task<bool> ActualizarAdministradorAsync(int id, string nuevoUsername, string nuevaPassword = null)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            if (!string.IsNullOrEmpty(nuevaPassword))
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);
                const string query = @"
                    UPDATE usuario 
                    SET username = @username, password = @password 
                    WHERE id = @id AND rol = 'Administrador'";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", nuevoUsername);
                cmd.Parameters.AddWithValue("@password", passwordHash);
                cmd.Parameters.AddWithValue("@id", id);

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
            else
            {
                const string query = @"
                    UPDATE usuario 
                    SET username = @username 
                    WHERE id = @id AND rol = 'Administrador'";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", nuevoUsername);
                cmd.Parameters.AddWithValue("@id", id);

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
