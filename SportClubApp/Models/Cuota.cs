// ============================================
// CLASES CUOTA Y PAGO
// SportClubApp - Models/Cuota.cs
// ============================================

namespace SportClubApp.Models
{
    // ============================================
    // CLASE: Cuota
    // ============================================
    public class Cuota
    {
        // Propiedades
        public int Id { get; set; }
        public int SocioId { get; set; }
        public Socio Socio { get; set; } // Navegación al Socio
        public double Monto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public EstadoCuota Estado { get; set; }
        public TipoCuota TipoCuota { get; set; }
        public MetodoPago? MetodoPago { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Constructor vacío
        public Cuota()
        {
            Estado = EstadoCuota.Pendiente;
            FechaCreacion = DateTime.Now;
        }

        // Constructor con parámetros
        public Cuota(int socioId, double monto, DateTime fechaVencimiento, TipoCuota tipo)
        {
            SocioId = socioId;
            Monto = monto;
            FechaVencimiento = fechaVencimiento;
            TipoCuota = tipo;
            Estado = EstadoCuota.Pendiente;
            FechaCreacion = DateTime.Now;
        }

        // ============================================
        // MÉTODOS DE NEGOCIO
        // ============================================

        /// <summary>
        /// Verifica si la cuota está vencida
        /// </summary>
        public bool EstaVencida()
        {
            return FechaVencimiento < DateTime.Now && Estado == EstadoCuota.Pendiente;
        }

        /// <summary>
        /// Calcula los días que faltan para el vencimiento (negativo si ya venció)
        /// </summary>
        public int DiasParaVencimiento()
        {
            return (FechaVencimiento - DateTime.Now).Days;
        }

        /// <summary>
        /// Verifica si está próxima a vencer (dentro de X días)
        /// </summary>
        public bool EstaProximaAVencer(int diasAntes = 5)
        {
            int dias = DiasParaVencimiento();
            return dias >= 0 && dias <= diasAntes && Estado == EstadoCuota.Pendiente;
        }

        /// <summary>
        /// Marca la cuota como pagada
        /// </summary>
        public void MarcarComoPagada(MetodoPago metodo)
        {
            Estado = EstadoCuota.Pagada;
            MetodoPago = metodo;
        }

        /// <summary>
        /// Marca la cuota como cancelada
        /// </summary>
        public void MarcarComoCancelada()
        {
            Estado = EstadoCuota.Cancelada;
        }

        /// <summary>
        /// Actualiza el estado a Vencida si corresponde
        /// </summary>
        public void ActualizarEstado()
        {
            if (EstaVencida() && Estado == EstadoCuota.Pendiente)
            {
                Estado = EstadoCuota.Vencida;
            }
        }

        /// <summary>
        /// Calcula el recargo por mora (2% por día vencido)
        /// </summary>
        public double CalcularRecargo()
        {
            if (!EstaVencida())
                return 0;

            int diasVencidos = Math.Abs(DiasParaVencimiento());
            double porcentajeRecargo = 0.02; // 2% por día

            return Monto * (diasVencidos * porcentajeRecargo);
        }

        /// <summary>
        /// Calcula el monto total (monto + recargo)
        /// </summary>
        public double MontoTotal()
        {
            return Monto + CalcularRecargo();
        }

        // ============================================
        // MÉTODOS DE VALIDACIÓN
        // ============================================

        public bool Validar(out string mensajeError)
        {
            if (SocioId <= 0)
            {
                mensajeError = "Debe especificar un socio válido";
                return false;
            }

            if (Monto <= 0)
            {
                mensajeError = "El monto debe ser mayor a cero";
                return false;
            }

            if (FechaVencimiento < FechaCreacion.Date)
            {
                mensajeError = "La fecha de vencimiento no puede ser anterior a la fecha de creación";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }

        // ============================================
        // MÉTODOS DE PRESENTACIÓN
        // ============================================

        public string ObtenerEstadoTexto()
        {
            return Estado switch
            {
                EstadoCuota.Pendiente => "Pendiente",
                EstadoCuota.Pagada => "Pagada",
                EstadoCuota.Vencida => "Vencida",
                EstadoCuota.Cancelada => "Cancelada",
                _ => "Desconocido"
            };
        }

        public string ObtenerTipoTexto()
        {
            return TipoCuota switch
            {
                TipoCuota.Mensual => "Mensual",
                TipoCuota.Diaria => "Diaria",
                TipoCuota.Anual => "Anual",
                TipoCuota.Actividad => "Actividad",
                _ => "Desconocido"
            };
        }

        public override string ToString()
        {
            return $"Cuota {ObtenerTipoTexto()} - Vencimiento: {FechaVencimiento.ToShortDateString()} - Monto: ${Monto:N2} - Estado: {ObtenerEstadoTexto()}";
        }
    }

    // ============================================
    // CLASE: Pago
    // ============================================
    public class Pago
    {
        // Propiedades
        public int Id { get; set; }
        public DateTime FechaPago { get; set; }
        public double Monto { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public int? CuotaId { get; set; }
        public Cuota Cuota { get; set; } // Navegación a Cuota
        public int SocioId { get; set; }
        public Socio Socio { get; set; } // Navegación a Socio
        public string Observaciones { get; set; }
        public string NumeroComprobante { get; set; }

        // Constructor vacío
        public Pago()
        {
            FechaPago = DateTime.Now;
        }

        // Constructor con parámetros
        public Pago(double monto, MetodoPago metodo, int socioId, int? cuotaId = null)
        {
            FechaPago = DateTime.Now;
            Monto = monto;
            MetodoPago = metodo;
            SocioId = socioId;
            CuotaId = cuotaId;
        }

        // Constructor completo
        public Pago(double monto, MetodoPago metodo, int socioId, int? cuotaId, string observaciones, string numeroComprobante = null)
        {
            FechaPago = DateTime.Now;
            Monto = monto;
            MetodoPago = metodo;
            SocioId = socioId;
            CuotaId = cuotaId;
            Observaciones = observaciones;
            NumeroComprobante = numeroComprobante ?? GenerarNumeroComprobante();
        }

        // ============================================
        // MÉTODOS DE NEGOCIO
        // ============================================

        /// <summary>
        /// Genera un número de comprobante único
        /// </summary>
        public string GenerarNumeroComprobante()
        {
            return $"PAGO-{DateTime.Now:yyyyMMddHHmmss}-{SocioId}";
        }

        /// <summary>
        /// Verifica si el pago corresponde a una cuota
        /// </summary>
        public bool EsPagoDeCuota()
        {
            return CuotaId.HasValue;
        }

        // ============================================
        // MÉTODOS DE VALIDACIÓN
        // ============================================

        public bool Validar(out string mensajeError)
        {
            if (Monto <= 0)
            {
                mensajeError = "El monto debe ser mayor a cero";
                return false;
            }

            if (SocioId <= 0)
            {
                mensajeError = "Debe especificar un socio válido";
                return false;
            }

            if (FechaPago > DateTime.Now)
            {
                mensajeError = "La fecha de pago no puede ser futura";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }

        // ============================================
        // MÉTODOS DE PRESENTACIÓN
        // ============================================

        public string ObtenerMetodoPagoTexto()
        {
            return MetodoPago switch
            {
                Models.MetodoPago.Efectivo => "Efectivo",
                Models.MetodoPago.TarjetaDebito => "Tarjeta de Débito",
                Models.MetodoPago.TarjetaCredito => "Tarjeta de Crédito",
                Models.MetodoPago.Transferencia => "Transferencia",
                Models.MetodoPago.MercadoPago => "Mercado Pago",
                Models.MetodoPago.Stripe => "Stripe",
                _ => "Desconocido"
            };
        }

        public override string ToString()
        {
            return $"Pago - Fecha: {FechaPago.ToShortDateString()} - Monto: ${Monto:N2} - Método: {ObtenerMetodoPagoTexto()}";
        }
    }
}
