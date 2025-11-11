using System;
using System.Collections.Generic;

namespace IngameScript
{
    public class MyAppServices : IMyAppServices
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public void AddSingleton<TService>(TService implementation) where TService : class
        {
            _services[typeof(TService)] = implementation;
        }

        public TService GetService<TService>() where TService : class
        {
            object service;
            if (_services.TryGetValue(typeof(TService), out service))
            {
                return (TService)service;
            }
            return null;
        }
    }
}