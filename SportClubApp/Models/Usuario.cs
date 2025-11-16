// ============================================
// CLASE USUARIO
// SportClubApp - Models/Usuario.cs
// ============================================

namespace SportClubApp.Models
{
    // ============================================
    // CLASE: Usuario
    // ============================================
    public class Usuario
    {
        // Propiedades
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Hash BCrypt
        public Rol Rol { get; set; }
        public bool Activo { get; set; }
        public int? PersonaId { get; set; }
        public Persona Persona { get; set; } // Navegación a Persona
        public DateTime FechaCreacion { get; set; }

        // Constructor vacío
        public Usuario()
        {
            Activo = true;
            FechaCreacion = DateTime.Now;
        }

        // Constructor con parámetros
        public Usuario(string username, string password, Rol rol)
        {
            Username = username;
            Password = password;
            Rol = rol;
            Activo = true;
            FechaCreacion = DateTime.Now;
        }

        // Constructor completo con Persona
        public Usuario(string username, string password, Rol rol, int personaId)
        {
            Username = username;
            Password = password;
            Rol = rol;
            PersonaId = personaId;
            Activo = true;
            FechaCreacion = DateTime.Now;
        }

        // ============================================
        // MÉTODOS DE VALIDACIÓN
        // ============================================

        public bool Validar(out string mensajeError)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                mensajeError = "El nombre de usuario es obligatorio";
                return false;
            }

            if (Username.Length < 4)
            {
                mensajeError = "El nombre de usuario debe tener al menos 4 caracteres";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                mensajeError = "La contraseña es obligatoria";
                return false;
            }

            if (Password.Length < 6)
            {
                mensajeError = "La contraseña debe tener al menos 6 caracteres";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }

        // ============================================
        // MÉTODOS DE NEGOCIO
        // ============================================

        public bool EsAdministrador()
        {
            return Rol == Rol.Administrador;
        }

        public bool EsEmpleado()
        {
            return Rol == Rol.Empleado;
        }

        public bool EsSocio()
        {
            return Rol == Rol.Socio;
        }

        public bool EsNoSocio()
        {
            return Rol == Rol.NoSocio;
        }

        public void Activar()
        {
            Activo = true;
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public bool PuedeAcceder()
        {
            return Activo;
        }

        // ============================================
        // MÉTODOS DE PRESENTACIÓN
        // ============================================

        public string ObtenerNombreRol()
        {
            return Rol switch
            {
                Rol.Administrador => "Administrador",
                Rol.Empleado => "Empleado",
                Rol.Socio => "Socio",
                Rol.NoSocio => "No Socio",
                _ => "Desconocido"
            };
        }

        public override string ToString()
        {
            return $"{Username} - {ObtenerNombreRol()} - {(Activo ? "Activo" : "Inactivo")}";
        }
    }
}
