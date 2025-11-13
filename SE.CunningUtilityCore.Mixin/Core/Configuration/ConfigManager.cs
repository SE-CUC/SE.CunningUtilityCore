using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript.Core.Configuration
{
    public class ConfigManager
    {
        private readonly MyGridProgram _program;
        private readonly Action<string> _echo;

        public ConfigManager(MyGridProgram program, Action<string> echo)
        {
            _program = program;
            _echo = echo;
        }

        public LoggerConfig Load()
        {
            var ini = new MyIni();
            var iniParsed = ini.TryParse(_program.Me.CustomData);

            if (!iniParsed && !string.IsNullOrWhiteSpace(_program.Me.CustomData))
            {
                _echo("[ERROR] Failed to parse CustomData configuration.");
                LoggerConfig.WriteDefault(ini);
                _program.Me.CustomData = ini.ToString();
                _echo("[INFO] Default configuration has been written to CustomData.");
            }

            return LoggerConfig.Read(ini);
        }
    }
}
