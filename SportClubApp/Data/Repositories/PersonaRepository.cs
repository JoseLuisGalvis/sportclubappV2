// Data/Repositories/PersonaRepository.cs
using MySql.Data.MySqlClient;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Models;
using System.Data;
using System.Text;

namespace SportClubApp.Data.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly IDatabaseConnection _dbConnection;

        public PersonaRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> ExistePersonaPorDniAsync(string dni)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "SELECT COUNT(1) FROM persona WHERE dni = @dni";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@dni", dni);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<int> CrearPersonaAsync(Persona persona)
        {
            if (!persona.Validar(out string mensajeError))
                throw new ArgumentException(mensajeError);

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                INSERT INTO persona (nombre, apellido, dni, telefono, email, tipo_persona, fecha_registro) 
                VALUES (@nombre, @apellido, @dni, @telefono, @email, @tipo_persona, @fecha_registro);
                SELECT LAST_INSERT_ID();";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
            cmd.Parameters.AddWithValue("@apellido", persona.Apellido);
            cmd.Parameters.AddWithValue("@dni", persona.Dni);
            cmd.Parameters.AddWithValue("@telefono", persona.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", persona.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@tipo_persona", persona.ObtenerTipo());
            cmd.Parameters.AddWithValue("@fecha_registro", persona.FechaRegistro);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            persona.Id = id;
            return id;
        }

        public async Task<Persona> ObtenerPersonaPorIdAsync(int id)
        {
            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                SELECT id, nombre, apellido, dni, telefono, email, tipo_persona, fecha_registro 
                FROM persona WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                string tipo = reader["tipo_persona"].ToString();

                if (tipo == "Socio")
                {
                    return new Socio
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Dni = reader.GetString("dni"),
                        Telefono = reader.IsDBNull("telefono") ? null : reader.GetString("telefono"),
                        Email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    };
                }
                else
                {
                    return new NoSocio
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Dni = reader.GetString("dni"),
                        Telefono = reader.IsDBNull("telefono") ? null : reader.GetString("telefono"),
                        Email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    };
                }
            }
            return null;
        }

        public async Task<bool> ActualizarPersonaAsync(Persona persona)
        {
            if (!persona.Validar(out string mensajeError))
                throw new ArgumentException(mensajeError);

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = @"
                UPDATE persona 
                SET nombre = @nombre, apellido = @apellido, telefono = @telefono, email = @email 
                WHERE id = @id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
            cmd.Parameters.AddWithValue("@apellido", persona.Apellido);
            cmd.Parameters.AddWithValue("@telefono", persona.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", persona.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", persona.Id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<Persona>> ObtenerTodasLasPersonasAsync()
        {
            var personas = new List<Persona>();

            using var conn = _dbConnection.GetConnection();
            await conn.OpenAsync();

            const string query = "SELECT id, nombre, apellido, dni, telefono, email, tipo_persona, fecha_registro FROM persona";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string tipo = reader["tipo_persona"].ToString();
                Persona persona;

                if (tipo == "Socio")
                {
                    persona = new Socio
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Dni = reader.GetString("dni"),
                        Telefono = reader.IsDBNull("telefono") ? null : reader.GetString("telefono"),
                        Email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    };
                }
                else
                {
                    persona = new NoSocio
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Dni = reader.GetString("dni"),
                        Telefono = reader.IsDBNull("telefono") ? null : reader.GetString("telefono"),
                        Email = reader.IsDBNull("email") ? null : reader.GetString("email"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    };
                }
                personas.Add(persona);
            }

            return personas;
        }
    }
}