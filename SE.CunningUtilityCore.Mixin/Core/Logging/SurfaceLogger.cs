using Sandbox.ModAPI.Ingame;
using System;
using System.Text;

namespace IngameScript
{
    public class SurfaceLogger : ILogger
    {
        private readonly LoggerConfig _config;
        private readonly IMyTextSurface _surface;
        private readonly StringBuilder _logCache = new StringBuilder();

        public SurfaceLogger(LoggerConfig config, IMyTextSurface surface)
        {
            _config = config;
            _surface = surface;
        }

        public void Write(LogLevel level, string text)
        {
            if (_surface == null || level > _config.LogLevel) return;

            var message = $"[{level.ToString().ToUpper()}] {text}\n";

            // Simple cache trimming
            if (_logCache.Length > 10000)
            {
                _logCache.Remove(10000, _logCache.Length - 10000);
            }
            _surface.WriteText(_logCache);
        }

        public void Debug(string text) => Write(LogLevel.Debug, text);
        public void Info(string text) => Write(LogLevel.Info, text);

        public void Error(string text) => Write(LogLevel.Error, text);
        public void Error(Exception e, string text = "") => Write(LogLevel.Error, $"{text}\n{e.Message}\n{e.StackTrace}");

        // These are for convenience and compatibility with older logger calls
        public void Debug(string format, params object[] args) => Write(LogLevel.Debug, string.Format(format, args));
        public void Info(string format, params object[] args) => Write(LogLevel.Info, string.Format(format, args));
        public void Error(string format, params object[] args) => Write(LogLevel.Error, string.Format(format, args));
    }
}
