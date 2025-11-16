using dotenv.net;

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
        static Config()
        {
            try
            {
                DotEnv.AutoConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Error cargando .env: {ex.Message}");
            }
        }


        public static readonly string STRIPE_SECRET_KEY =
            Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY")
            ?? throw new Exception("❌ STRIPE_SECRET_KEY no encontrada en .env");

        public static readonly string STRIPE_PUBLISHABLE_KEY =
            Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY")
            ?? throw new Exception("❌ STRIPE_PUBLISHABLE_KEY no encontrada en .env");

        // ============================================
        // MONTOS Y URLS
        // ============================================
        public const long MEMBERSHIP_AMOUNT_CENTS = 10000000; // $100.000 (Stripe usa centavos)
        public const string CURRENCY = "USD";

        public const string SUCCESS_URL = "https://localhost:5001/success";
        public const string CANCEL_URL = "https://localhost:5001/cancel";

        public const long NOSOCIO_AMOUNT_CENTS = 1000000; // $10.000
        public const string NOSOCIO_PRODUCT_NAME = "Entrada Diaria - No Socio";
        public const string NOSOCIO_SUCCESS_URL = "https://localhost:5001/success";
        public const string NOSOCIO_CANCEL_URL = "https://localhost:5001/cancel";

    }
}

