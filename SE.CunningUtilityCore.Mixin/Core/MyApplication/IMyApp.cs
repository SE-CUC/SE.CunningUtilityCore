using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public interface IMyApp
    {
        void Main(string argument, UpdateType updateSource);
        void Save();
    }
}