using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClubApp
{
    public static class Config
    {
        // ============================================
        // BASE DE DATOS
        // ============================================
        public static string ConnectionString =>
            "Server=localhost;Port=3306;Database=clubdeportivoG5;User=root;Password=root;";

        // ============================================
        // STRIPE CONFIGURATION
        // ============================================
        public static readonly string STRIPE_SECRET_KEY = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
        public static readonly string STRIPE_PUBLISHABLE_KEY = Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY");

        // Monto de membresía en centavos (100.000 pesos = 10.000.000 centavos)
        public const long MEMBERSHIP_AMOUNT_CENTS = 10000000;
        public const string CURRENCY = "usd"; // Peso Argentino

        // URLs de retorno (localhost para testing)
        public const string SUCCESS_URL = "http://localhost:5000/success";
        public const string CANCEL_URL = "http://localhost:5000/cancel";
    }
}

