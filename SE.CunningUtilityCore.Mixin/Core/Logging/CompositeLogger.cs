using System;
using System.Collections.Generic;
using System.Linq;

namespace IngameScript
{
    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger(params ILogger[] loggers)
        {
            _loggers = loggers.ToList();
        }

        public void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public void Write(LogLevel level, string text) =>
            _loggers.ForEach(l => l.Write(level, text));

        public void Debug(string text) => Write(LogLevel.Debug, text);

        public void Info(string text) => Write(LogLevel.Info, text);

        public void Error(string text) => Write(LogLevel.Error, text);
        public void Error(Exception e, string text = "")
        {
            _loggers.ForEach(l => l.Error(e, text));
        }
    }
}
