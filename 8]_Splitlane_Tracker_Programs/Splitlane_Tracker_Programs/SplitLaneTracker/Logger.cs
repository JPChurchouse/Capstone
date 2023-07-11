using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Services.Logging
{
  public class Logger
  {
    // CONSTRUCTOR, DESTRUCTOR //
    /// <summary>
    /// Create new Logger object
    /// Logger automatically selects file location
    /// </summary>
    public Logger()
    {
      init();
    }
    ~Logger()
    {
      Log.Information($"Closing");
      Log.CloseAndFlush();
    }

    // PUBLIC VARS AND FUNCS //
    /// <summary>
    /// Log message type
    /// </summary>
    public enum Type
    {
      debug,
      info,
      error
    }

    /// <summary>
    /// Log a message
    /// </summary>
    /// <param name="info"> Message to log</param>
    /// <param name="type"> Type of message</param>
    public void log(string info, Type type = Type.debug)
    {
      switch (type)
      {
        case Type.debug:
          Log.Debug(info);
          break;

        case Type.info:
          Log.Information(info);
          break;

        case Type.error:
        default:
          Log.Error(info);
          break;
      }
    }

    /// <summary>
    /// Log an exception
    /// </summary>
    /// <param name="exc"></param>
    public void log(Exception exc)
    {
      log(exc.Message, Type.error);
    }

    /// <summary>
    /// Select the log file in File Explorer
    /// </summary>
    public void open()
    {
      try
      {
        Process.Start("explorer.exe", $"/select, {file}");
      }
      catch { }
    }

    // PRIVATE VARS AND FUNCS //
    // Log file name
    private string file = "logs/err.txt";

    // Initalise logging func
    private void init()
    {
      // Get program information
      Assembly? assembly = Assembly.GetEntryAssembly();
      string name = assembly?.GetName().Name ?? "ERROR";
      Version vers = assembly?.GetName().Version ?? new Version(0, 0);

      // Generate filepath
      string directory;
      #if DEBUG       // When in DEBUG mode, log directory = working directory
      directory = $@"{Environment.CurrentDirectory}\logs";
      #else         // When in RELEASE mode, log directory = user's Documents folder and has program name
      directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"\{name}_logs" ;
      #endif

      // Generate file
      string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
      file = $@"{directory}\{time}_{name}_v{vers}.log";

      // Create Log object
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console()
        .WriteTo.File(file)
        .CreateLogger();

      // Inital log information
      Log.Information("This program was developed by J. P. Churchouse");
      Log.Information($"User: {Environment.MachineName} - {Environment.UserName}");
      Log.Information($"EXE Path: {Environment.ProcessPath}");
      Log.Information($"LOG Path: {file}");
    }
  }
}
