using System;
using System.Windows.Forms;
using DotNetEnv;

namespace SportClubApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Cargar las variables de entorno desde el archivo .env
            Env.Load();

            // Inicializa la configuración de la app (fuentes, DPI, etc.)
            ApplicationConfiguration.Initialize();

            // Ejecuta el formulario principal
            Application.Run(new Form1());
        }
    }
}
