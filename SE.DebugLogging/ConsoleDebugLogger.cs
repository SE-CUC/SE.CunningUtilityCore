using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public class ConsoleDebugLogger: ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        public void Debug(string message)
        {
            Console.WriteLine($"[DEBUG] {message}");
        }

        public void Error(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }
    }

    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Error(string message);
    }
}
