using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitLaneTracker_Server
{
    internal class Programme
    {
        private string file_Logging;
        private void InitLog()
        {
            string timenow = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            file_Logging = $"logs\\{timenow}_SplitlaneTracker.log";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File(file_Logging)
                .CreateLogger();
            Log.Information("This programme was developed by J. P. Churchouse");
            Log.Information("Started programme at time: " + timenow);
        }
        public void OpenLogFile()
        {
            try { Process.Start("explorer.exe", $"/select, {Environment.CurrentDirectory}\\{file_Logging}"); }
            catch { }
        }
    }
}
