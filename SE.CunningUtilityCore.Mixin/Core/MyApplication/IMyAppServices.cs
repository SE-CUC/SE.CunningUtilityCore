using System;

namespace IngameScript
{
    public interface IMyAppServices
    {
        void AddSingleton<TService>(TService implementation) where TService : class;
        TService GetService<TService>() where TService : class;
    }
}