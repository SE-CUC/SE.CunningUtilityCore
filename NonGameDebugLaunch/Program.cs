using IngameScript;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonGameDebugLaunch
{

    internal class Program
    {
        static void Main(string[] args)
        {
            //TestClass testClass;
            //ILogger logger = new MyConsoleTextSurface();
            //var surface = logger as Sandbox.ModAPI.Ingame.IMyTextSurface;
            //MyGridProgram myGridProgram = new MyConsoleGridProgram(surface);

            //testClass = new TestClass(logger, myGridProgram, surface);
            //testClass.DoSomething();

            Console.ReadLine();
        }
    }

    class MyConsoleGridProgram : MyGridProgram
    {
        public MyConsoleGridProgram(Sandbox.ModAPI.Ingame.IMyTextSurface surface)
        {
            Me = new MyConsoleProgramBlock(surface);
            Echo = Console.WriteLine;
        }
    }

    class MyConsoleProgramBlock : MyProgrammableBlock, Sandbox.ModAPI.Ingame.IMyProgrammableBlock
    {
        private readonly Sandbox.ModAPI.Ingame.IMyTextSurface _surface;

        public MyConsoleProgramBlock(Sandbox.ModAPI.Ingame.IMyTextSurface surface)
        {
            _surface = surface;
        }

        public new Sandbox.ModAPI.Ingame.IMyTextSurface GetSurface(int index)
        {
            return _surface;
        }
    }

    class MyConsoleTextSurface : MyTextPanel, Sandbox.ModAPI.Ingame.IMyTextSurface, Sandbox.ModAPI.IMyTextSurface, ILogger
    {
        public void Debug(string text)
        {
            Console.WriteLine("[DEBUG] " + text);
        }

        public void Error(string text)
        {
            Console.WriteLine("[ERROR] " + text);
        }

        public void Error(Exception e, string text = "")
        {
            Console.WriteLine("[ERROR] " + text + " Exception: " + e.ToString());
        }

        public void Info(string text)
        {
            Console.WriteLine("[INFO] " + text);
        }

        public void Write(LogLevel level, string text)
        {
            Console.WriteLine("[" + level.ToString().ToUpper() + "] " + text);
        }

        public bool WriteText(string value, bool append = false)
        {
            Console.WriteLine(value);
            return true;
        }

        public bool WriteText(StringBuilder value, bool append = false)
        {
            Console.WriteLine(value.ToString());
            return true;
        }
    }
}
