using Application.Interfaces;
using System;
using System.IO;


namespace Infrastructure.Services;

public class FileLoggerService : ILoggerService
{
    private readonly string _logDirectory;
    private readonly string _logFile;

    public FileLoggerService()
    {
        _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
        _logFile = Path.Combine(_logDirectory, $"log_{DateTime.UtcNow:yyyyMMdd}.txt");

        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }
    }

    public void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }

    public void LogWarning(string message)
    {
        WriteLog("WARN", message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        var fullMessage = ex != null ? $"{message} | Exception: {ex.Message}" : message;
        WriteLog("ERROR", fullMessage);
    }

    private void WriteLog(string level, string message)
    {
        var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
        File.AppendAllText(_logFile, logEntry + Environment.NewLine);
    }
}
