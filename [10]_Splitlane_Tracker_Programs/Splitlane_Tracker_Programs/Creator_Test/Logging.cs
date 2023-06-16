using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Creator_Test
{
    public partial class MainWindow : Window
    {
        private string file_Logging = "logs/err.txt";
        private void InitLog()
        {
            string? name = Assembly.GetCallingAssembly().GetName().Name;
            Version? vers = Assembly.GetCallingAssembly().GetName().Version;
            string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            file_Logging = $"logs/{time}_{name}_v{vers}.log";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File(file_Logging)
                .CreateLogger();

            Log.Information("This program was developed by J. P. Churchouse");
            Log.Information($"User: {Environment.MachineName}\\{Environment.UserName}");
            Log.Information($"Path: {Environment.ProcessPath}");
        }
    }
}
