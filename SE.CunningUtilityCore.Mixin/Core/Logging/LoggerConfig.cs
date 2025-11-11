using System;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    public class LoggerConfig
    {
        public const string SectionName = "Logging";
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        public static LoggerConfig GetDefault() => new LoggerConfig();

        public static void WriteDefault(MyIni ini)
        {
            var config = GetDefault();
            ini.Set(SectionName, nameof(LogLevel), config.LogLevel.ToString());
            ini.SetComment(SectionName, nameof(LogLevel), " Log level: None, Error, Info, Debug");
        }

        public static LoggerConfig Read(MyIni ini)
        {
            var config = GetDefault();
            var levelString = ini.Get(SectionName, nameof(LogLevel)).ToString(config.LogLevel.ToString());
            LogLevel level;
            Enum.TryParse(levelString, true, out level);
            config.LogLevel = level;
            return config;
        }
    }
}