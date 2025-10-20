using System;
using System.Windows.Forms;
using dotenv.net;
using System.IO;

namespace SportClubApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Cargar .env desde la carpeta raíz del proyecto
            string envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");

            if (File.Exists(envPath))
            {
                DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { envPath }));

                string key = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY") ?? "NO ENCONTRADA";
                MessageBox.Show("STRIPE_KEY: " + key);
            }
            else
            {
                MessageBox.Show("✗ .env NO encontrado en: " + envPath);
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
