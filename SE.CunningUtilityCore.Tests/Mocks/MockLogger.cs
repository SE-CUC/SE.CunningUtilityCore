using System;
using System.Collections.Generic;

namespace IngameScript.Tests.Mocks
{
    public class MockLogger : ILogger
    {
        public List<string> Messages { get; } = new List<string>();

        public void Write(LogLevel level, string text)
        {
            Messages.Add($"[{level.ToString().ToUpper()}] {text}");
        }

        public void Debug(string text)
        {
            Write(LogLevel.Debug, text);
        }

        public void Info(string text)
        {
            Write(LogLevel.Info, text);
        }

        public void Error(string text)
        {
            Write(LogLevel.Error, text);
        }

        public void Error(Exception e, string text = "")
        {
            Write(LogLevel.Error, $"{text} - {e.Message}");
        }
    }
}
