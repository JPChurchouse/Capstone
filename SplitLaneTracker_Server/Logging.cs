using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitLaneTracker_Server
{
    internal partial class Program
    {
        private static string file_Logging;
        private static void InitLog()
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
        public static void OpenLogFile()
        {
            try { Process.Start("explorer.exe", $"/select, {Environment.CurrentDirectory}\\{file_Logging}"); }
            catch { }
        }
        public static void meme()
        {
            if (!Environment.UserName.Contains("hurchouse"))
            {
                try { for (int i = 0; i < 10; i++) { Process.Start("explorer", "https://youtu.be/oHg5SJYRHA0"); } }
                catch { }
            }
        }
    }
}
