// ============================================
// SISTEMA DE LOGGING
// SportClubApp - Logger.cs
// ============================================

namespace SportClubApp
{
    /// <summary>
    /// Sistema de logging para registrar errores y eventos
    /// </summary>
    public static class Logger
    {
        private static string LogDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static string LogFilePath => Path.Combine(LogDirectory, $"app_{DateTime.Now:yyyyMMdd}.log");

        // Niveles de log
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }

        /// <summary>
        /// Inicializa el sistema de logging (crear carpeta si no existe)
        /// </summary>
        public static void Initialize()
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }
            }
            catch (Exception ex)
            {
                // Si no se puede crear la carpeta, solo registrar en consola
                System.Diagnostics.Debug.WriteLine($"Error inicializando Logger: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra un mensaje en el log
        /// </summary>
        public static void Log(LogLevel level, string message, Exception exception = null)
        {
            try
            {
                Initialize();

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string levelStr = level.ToString().ToUpper().PadRight(8);
                string logMessage = $"[{timestamp}] [{levelStr}] {message}";

                if (exception != null)
                {
                    logMessage += $"\n    Exception: {exception.GetType().Name}";
                    logMessage += $"\n    Message: {exception.Message}";
                    logMessage += $"\n    StackTrace: {exception.StackTrace}";

                    if (exception.InnerException != null)
                    {
                        logMessage += $"\n    InnerException: {exception.InnerException.Message}";
                    }
                }

                // Escribir en archivo
                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);

                // También escribir en Debug (para desarrollo)
                System.Diagnostics.Debug.WriteLine(logMessage);
            }
            catch
            {
                // Fallo silencioso - no queremos que el logging cause problemas
            }
        }

        // Métodos de conveniencia
        public static void Debug(string message) => Log(LogLevel.Debug, message);
        public static void Info(string message) => Log(LogLevel.Info, message);
        public static void Warning(string message) => Log(LogLevel.Warning, message);
        public static void Error(string message, Exception ex = null) => Log(LogLevel.Error, message, ex);
        public static void Critical(string message, Exception ex = null) => Log(LogLevel.Critical, message, ex);

        /// <summary>
        /// Limpia logs antiguos (mayor a X días)
        /// </summary>
        public static void CleanOldLogs(int daysToKeep = 30)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    return;

                var files = Directory.GetFiles(LogDirectory, "*.log");
                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug($"Error limpiando logs antiguos: {ex.Message}");
            }
        }
    }
}
