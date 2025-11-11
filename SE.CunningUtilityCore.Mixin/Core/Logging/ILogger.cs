using System;

namespace IngameScript
{
    public interface ILogger
    {
        void Write(LogLevel level, string text);
        void Debug(string text);
        void Info(string text);
        void Error(string text);
        void Error(Exception e, string text = "");
    }
}
