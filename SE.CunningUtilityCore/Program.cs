﻿using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        private IMyApp _app;

        public Program()
        {
            _app = MyAppBuilder.Create(this).Build();
        }

        public void Main(string argument, UpdateType updateSource) => _app.Main(argument, updateSource);

        public void Save() => _app.Save();
    }
}
