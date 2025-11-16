// ============================================
// CLASES DE PERSONAS
// SportClubApp - Models/Persona.cs
// ============================================

namespace SportClubApp.Models
{
    // ============================================
    // CLASE ABSTRACTA: Persona
    // ============================================
    public abstract class Persona
    {
        // Propiedades
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Constructor vacío
        protected Persona()
        {
            FechaRegistro = DateTime.Now;
        }

        // Constructor con parámetros
        protected Persona(string nombre, string apellido, string dni, string telefono = null, string email = null)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            Telefono = telefono;
            Email = email;
            FechaRegistro = DateTime.Now;
        }

        // Método abstracto - cada clase hija debe implementarlo
        public abstract string ObtenerTipo();

        // Método para obtener nombre completo
        public string NombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        // Método de validación
        public virtual bool Validar(out string mensajeError)
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                mensajeError = "El nombre es obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Apellido))
            {
                mensajeError = "El apellido es obligatorio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Dni))
            {
                mensajeError = "El DNI es obligatorio";
                return false;
            }

            if (Dni.Length < 7 || Dni.Length > 8)
            {
                mensajeError = "El DNI debe tener entre 7 y 8 dígitos";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
            {
                mensajeError = "El email no es válido";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }

        public override string ToString()
        {
            return $"{NombreCompleto()} - DNI: {Dni}";
        }
    }

    // ============================================
    // CLASE: Socio (hereda de Persona)
    // ============================================
    public class Socio : Persona
    {
        // ========================================
        // PROPIEDADES ESPECÍFICAS DE SOCIO
        // ========================================

        public int NroSocio { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Habilitado { get; set; }
        public bool AptoFisico { get; set; }
        public string Carnet { get; set; }

        // ✅ NUEVA PROPIEDAD: Foto del carnet (almacenada como byte array)
        public byte[] FotoCarnet { get; set; }

        // ========================================
        // PROPIEDADES PARA STRIPE (PAGOS)
        // ========================================

        public string StripeSessionId { get; set; }
        public string StripePaymentIntentId { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public DateTime? FechaPago { get; set; }
        public double MontoPago { get; set; }

        // ========================================
        // RELACIONES
        // ========================================

        public List<Cuota> Cuotas { get; set; }

        // ========================================
        // CONSTRUCTORES
        // ========================================

        /// <summary>
        /// Constructor vacío
        /// </summary>
        public Socio() : base()
        {
            FechaAlta = DateTime.Now;
            Habilitado = false; // Se habilita después del pago
            AptoFisico = false;
            EstadoPago = EstadoPago.Pendiente;
            MontoPago = 100000.00; // Monto por defecto
            Cuotas = new List<Cuota>();
            FotoCarnet = null;
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        public Socio(string nombre, string apellido, string dni, string telefono = null, string email = null)
            : base(nombre, apellido, dni, telefono, email)
        {
            FechaAlta = DateTime.Now;
            Habilitado = false;
            AptoFisico = false;
            EstadoPago = EstadoPago.Pendiente;
            MontoPago = 100000.00;
            Cuotas = new List<Cuota>();
            FotoCarnet = null;
        }

        // ========================================
        // MÉTODOS HEREDADOS (ABSTRACTOS)
        // ========================================

        public override string ObtenerTipo()
        {
            return "Socio";
        }

        // ========================================
        // MÉTODOS ESPECÍFICOS DE SOCIO
        // ========================================

        /// <summary>
        /// Genera el número de carnet en formato SOC-00001
        /// </summary>
        public void GenerarCarnet()
        {
            Carnet = $"SOC-{NroSocio:D5}";
        }

        /// <summary>
        /// Verifica si el socio puede acceder al club
        /// </summary>
        public bool PuedeAcceder()
        {
            return Habilitado && EstadoPago == EstadoPago.Completado;
        }

        /// <summary>
        /// Habilita al socio
        /// </summary>
        public void HabilitarSocio()
        {
            Habilitado = true;
        }

        /// <summary>
        /// Deshabilita al socio
        /// </summary>
        public void DeshabilitarSocio()
        {
            Habilitado = false;
        }

        /// <summary>
        /// Marca el pago como completado
        /// </summary>
        public void MarcarPagoCompletado(string paymentIntentId)
        {
            EstadoPago = EstadoPago.Completado;
            StripePaymentIntentId = paymentIntentId;
            FechaPago = DateTime.Now;
            Habilitado = true;
        }

        /// <summary>
        /// Agrega una cuota al socio
        /// </summary>
        public void AgregarCuota(Cuota cuota)
        {
            if (Cuotas == null)
                Cuotas = new List<Cuota>();

            Cuotas.Add(cuota);
        }

        /// <summary>
        /// Verifica si tiene cuotas vencidas
        /// </summary>
        public bool TieneCuotasVencidas()
        {
            if (Cuotas == null || Cuotas.Count == 0)
                return false;

            return Cuotas.Exists(c => c.EstaVencida() && c.Estado == EstadoCuota.Pendiente);
        }

        /// <summary>
        /// Obtiene las cuotas vencidas
        /// </summary>
        public List<Cuota> ObtenerCuotasVencidas()
        {
            if (Cuotas == null)
                return new List<Cuota>();

            return Cuotas.FindAll(c => c.EstaVencida() && c.Estado == EstadoCuota.Pendiente);
        }

        /// <summary>
        /// Calcula el total de deuda (incluyendo recargos)
        /// </summary>
        public double CalcularDeudaTotal()
        {
            if (Cuotas == null || Cuotas.Count == 0)
                return 0;

            double total = 0;
            foreach (var cuota in Cuotas)
            {
                if (cuota.Estado == EstadoCuota.Pendiente || cuota.Estado == EstadoCuota.Vencida)
                {
                    total += cuota.MontoTotal();
                }
            }
            return total;
        }

        // ========================================
        // VALIDACIÓN
        // ========================================

        public override bool Validar(out string mensajeError)
        {
            // Validaciones base de Persona
            if (!base.Validar(out mensajeError))
                return false;

            // Validaciones específicas de Socio
            if (FechaAlta > DateTime.Now)
            {
                mensajeError = "La fecha de alta no puede ser futura";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }

        // ========================================
        // TO STRING
        // ========================================

        public override string ToString()
        {
            return $"{base.ToString()} - Carnet: {Carnet} - Estado: {(Habilitado ? "Habilitado" : "Deshabilitado")}";
        }
    }

    // ============================================
    // CLASE: NoSocio (hereda de Persona)
    // ============================================
    public class NoSocio : Persona
    {
        // ========================================
        // PROPIEDADES ESPECÍFICAS DE NOSOCIO
        // ========================================
        public int IdNoSocio { get; set; }
        public bool Habilitado { get; set; }
        public DateTime? FechaVisita { get; set; }
        public byte[] FotoVisita { get; set; }

        // ========================================
        // PROPIEDADES PARA STRIPE (PAGO DIARIO)
        // ========================================
        public string StripeSessionId { get; set; }
        public string StripePaymentIntentId { get; set; }
        public EstadoPago EstadoPago { get; set; }
        public DateTime? FechaPago { get; set; }
        public double MontoPago { get; set; }

        // ========================================
        // CONSTRUCTORES
        // ========================================

        /// <summary>
        /// Constructor vacío
        /// </summary>
        public NoSocio() : base()
        {
            Habilitado = false;
            FechaVisita = DateTime.Now;
            EstadoPago = EstadoPago.Pendiente;
            MontoPago = 10000.00; // Monto por defecto para visita diaria
            FotoVisita = null;
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        public NoSocio(string nombre, string apellido, string dni, string telefono = null, string email = null)
            : base(nombre, apellido, dni, telefono, email)
        {
            Habilitado = false;
            FechaVisita = DateTime.Now;
            EstadoPago = EstadoPago.Pendiente;
            MontoPago = 10000.00;
            FotoVisita = null;
        }

        // ========================================
        // MÉTODOS HEREDADOS (ABSTRACTOS)
        // ========================================

        public override string ObtenerTipo()
        {
            return "NoSocio";
        }

        // ========================================
        // MÉTODOS ESPECÍFICOS DE NOSOCIO
        // ========================================

        /// <summary>
        /// Verifica si el no socio puede acceder al club
        /// </summary>
        public bool PuedeAcceder()
        {
            return Habilitado && EstadoPago == EstadoPago.Completado;
        }

        /// <summary>
        /// Registra una nueva visita
        /// </summary>
        public void RegistrarVisita()
        {
            FechaVisita = DateTime.Now;
        }

        /// <summary>
        /// Calcula los días desde la última visita
        /// </summary>
        public int DiasDesdeUltimaVisita()
        {
            if (!FechaVisita.HasValue)
                return -1;

            return (DateTime.Now - FechaVisita.Value).Days;
        }

        /// <summary>
        /// Marca el pago como completado
        /// </summary>
        public void MarcarPagoCompletado(string paymentIntentId)
        {
            EstadoPago = EstadoPago.Completado;
            StripePaymentIntentId = paymentIntentId;
            FechaPago = DateTime.Now;
            Habilitado = true;
        }

        /// <summary>
        /// Habilita al no socio
        /// </summary>
        public void HabilitarNoSocio()
        {
            Habilitado = true;
        }

        /// <summary>
        /// Deshabilita al no socio
        /// </summary>
        public void DeshabilitarNoSocio()
        {
            Habilitado = false;
        }

        // ========================================
        // VALIDACIÓN
        // ========================================

        public override bool Validar(out string mensajeError)
        {
            // Validaciones base de Persona
            if (!base.Validar(out mensajeError))
                return false;

            // Validaciones específicas de NoSocio
            //if (FechaVisita.HasValue && FechaVisita.Value > DateTime.Now)
            //{
                //mensajeError = "La fecha de visita no puede ser futura";
                //return false;
            //}

            mensajeError = string.Empty;
            return true;
        }

        // ========================================
        // TO STRING
        // ========================================

        public override string ToString()
        {
            return $"{base.ToString()} - Última visita: {FechaVisita?.ToShortDateString() ?? "Sin registro"} - Estado: {(Habilitado ? "Habilitado" : "Deshabilitado")}";
        }
    }
}