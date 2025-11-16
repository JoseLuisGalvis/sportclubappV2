using dotenv.net;
using Microsoft.Extensions.DependencyInjection;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Data.Repositories;

using dotenv.net;
using Microsoft.Extensions.DependencyInjection;
using SportClubApp.Data.Database;
using SportClubApp.Data.Interfaces;
using SportClubApp.Data.Repositories;

namespace SportClubApp
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // ===== MOSTRAR SPLASH SCREEN =====
            using (var splash = new SplashScreenForm())
            {
                splash.Show();
                Application.DoEvents();

                // Mientras se muestra el splash, cargar configuraciones
                CargarConfiguraciones();
                ConfigureServices();

                // Esperar a que termine el timer del splash
                while (splash.Visible)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
            }

            // ===== INICIAR APLICACIÓN PRINCIPAL =====
            using (var scope = ServiceProvider.CreateScope())
            {
                var form1 = scope.ServiceProvider.GetRequiredService<Form1>();
                Application.Run(form1);
            }
        }

        static void CargarConfiguraciones()
        {
            // Cargar .env desde la carpeta raíz del proyecto
            string envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
            if (File.Exists(envPath))
            {
                DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { envPath }));
                // Comentar o eliminar el MessageBox para no interrumpir el splash
                // string key = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY") ?? "NO ENCONTRADA";
                // MessageBox.Show("STRIPE_KEY: " + key);
            }
        }

        static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Database
            services.AddScoped<IDatabaseConnection>(_ =>
                new DatabaseConnection(Config.ConnectionString));

            // Repositories
            services.AddScoped<IPersonaRepository, PersonaRepository>();
            services.AddScoped<ISocioRepository, SocioRepository>();
            services.AddScoped<INoSocioRepository, NoSocioRepository>();
            services.AddScoped<ICuotaRepository, CuotaRepository>();
            services.AddScoped<IPagoRepository, PagoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IFotoRepository, FotoRepository>();

            // Forms
            services.AddTransient<Form1>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
