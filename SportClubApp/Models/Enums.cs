// ============================================
// ENUMERACIONES DEL SISTEMA
// SportClubApp - Models/Enums.cs
// ============================================

namespace SportClubApp.Models
{
    // ============================================
    // ENUM: Rol de Usuario
    // ============================================
    public enum Rol
    {
        Administrador,
        Empleado,
        Socio,
        NoSocio
    }

    // ============================================
    // ENUM: Estado de Pago (Stripe/Membresía)
    // ============================================
    public enum EstadoPago
    {
        Pendiente,
        Completado,
        Fallido
    }

    // ============================================
    // ENUM: Estado de Cuota
    // ============================================
    public enum EstadoCuota
    {
        Pendiente,
        Pagada,
        Vencida,
        Cancelada
    }

    // ============================================
    // ENUM: Tipo de Cuota
    // ============================================
    public enum TipoCuota
    {
        Mensual,
        Diaria,
        Anual,
        Actividad
    }

    // ============================================
    // ENUM: Método de Pago
    // ============================================
    public enum MetodoPago
    {
        Efectivo,
        TarjetaDebito,
        TarjetaCredito,
        Transferencia,
        MercadoPago,
        Stripe
    }
}
