using IngameScript;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components.Interfaces;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace NonGameDebugLaunch
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var logger = new MyConsoleTextSurface();
            var mcgp = new MyConsoleGridProgram(logger);
            var app = MyAppBuilder.Create(mcgp).Build();
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

    class MyConsoleProgramBlock : Sandbox.ModAPI.Ingame.IMyProgrammableBlock
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

        public string CustomData { get; set; }


        #region NotImplementedMembers

        public bool IsRunning => throw new NotImplementedException();

        public string TerminalRunArgument => throw new NotImplementedException();

        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CustomName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string CustomNameWithFaction => throw new NotImplementedException();

        public string DetailedInfo => throw new NotImplementedException();

        public string CustomInfo => throw new NotImplementedException();

        public bool ShowOnHUD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInTerminal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInToolbarConfig { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool ShowInInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SerializableDefinitionId BlockDefinition => throw new NotImplementedException();

        public IMyCubeGrid CubeGrid => throw new NotImplementedException();

        public string DefinitionDisplayNameText => throw new NotImplementedException();

        public float DisassembleRatio => throw new NotImplementedException();

        public string DisplayNameText => throw new NotImplementedException();

        public bool IsBeingHacked => throw new NotImplementedException();

        public bool IsFunctional => throw new NotImplementedException();

        public bool IsWorking => throw new NotImplementedException();

        public Vector3I Max => throw new NotImplementedException();

        public float Mass => throw new NotImplementedException();

        public Vector3I Min => throw new NotImplementedException();

        public int NumberInGrid => throw new NotImplementedException();

        public MyBlockOrientation Orientation => throw new NotImplementedException();

        public long OwnerId => throw new NotImplementedException();

        public Vector3I Position => throw new NotImplementedException();

        public IMyEntityComponentContainer Components => throw new NotImplementedException();

        public long EntityId => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public bool HasInventory => throw new NotImplementedException();

        public int InventoryCount => throw new NotImplementedException();

        public bool Closed => throw new NotImplementedException();

        public BoundingBoxD WorldAABB => throw new NotImplementedException();

        public BoundingBoxD WorldAABBHr => throw new NotImplementedException();

        public MatrixD WorldMatrix => throw new NotImplementedException();

        public BoundingSphereD WorldVolume => throw new NotImplementedException();

        public BoundingSphereD WorldVolumeHr => throw new NotImplementedException();

        public bool UseGenericLcd => throw new NotImplementedException();

        public int SurfaceCount => throw new NotImplementedException();

        public void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public ITerminalAction GetActionWithName(string name)
        {
            throw new NotImplementedException();
        }

        public IMyInventory GetInventory()
        {
            throw new NotImplementedException();
        }

        public IMyInventory GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        public string GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        public Vector3D GetPosition()
        {
            throw new NotImplementedException();
        }

        public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public ITerminalProperty GetProperty(string id)
        {
            throw new NotImplementedException();
        }

        

        public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public bool HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        public bool HasNobodyPlayerAccessToBlock()
        {
            throw new NotImplementedException();
        }

        public bool HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public bool HasPlayerAccessWithNobodyCheck(long playerId, bool isForPB = false)
        {
            throw new NotImplementedException();
        }

        public bool IsSameConstructAs(Sandbox.ModAPI.Ingame.IMyTerminalBlock other)
        {
            throw new NotImplementedException();
        }

        public void RequestEnable(bool enable)
        {
            throw new NotImplementedException();
        }

        public void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        public bool TryRun(string argument)
        {
            throw new NotImplementedException();
        }

        public void UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisual()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    class MyConsoleTextSurface : Sandbox.ModAPI.Ingame.IMyTextSurface, Sandbox.ModAPI.IMyTextSurface, ILogger
    {

        public void Error(string text)
        {
            Console.WriteLine("[ERROR] " + text);
        }
        public void Error(Exception e, string text = "")
        {
            Console.WriteLine("[ERROR] " + text + " Exception: " + e.ToString());
        }
        public void Debug(string text)
        {
            Console.WriteLine("[DEBUG] " + text);
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

        #region NotImplementedMembers

        public string CurrentlyShownImage => throw new NotImplementedException();

        public float FontSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color FontColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte BackgroundAlpha { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float ChangeInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Font { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TextAlignment Alignment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Script { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ContentType ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Vector2 SurfaceSize => throw new NotImplementedException();

        public Vector2 TextureSize => throw new NotImplementedException();

        public bool PreserveAspectRatio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float TextPadding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color ScriptBackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Color ScriptForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Name => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public void AddImagesToSelection(List<string> ids, bool checkExistence = false)
        {
            throw new NotImplementedException();
        }

        public void AddImageToSelection(string id, bool checkExistence = false)
        {
            throw new NotImplementedException();
        }

        public void ClearImagesFromSelection()
        {
            throw new NotImplementedException();
        }

        public MySpriteDrawFrame DrawFrame()
        {
            throw new NotImplementedException();
        }    

        public void GetFonts(List<string> fonts)
        {
            throw new NotImplementedException();
        }

        public void GetScripts(List<string> scripts)
        {
            throw new NotImplementedException();
        }

        public void GetSelectedImages(List<string> output)
        {
            throw new NotImplementedException();
        }

        public void GetSprites(List<string> sprites)
        {
            throw new NotImplementedException();
        }

        public string GetText()
        {
            throw new NotImplementedException();
        }

        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
        {
            throw new NotImplementedException();
        }

        public void ReadText(StringBuilder buffer, bool append = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveImageFromSelection(string id, bool removeDuplicates = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
        {
            throw new NotImplementedException();
        }

        #endregion 
    }
}
