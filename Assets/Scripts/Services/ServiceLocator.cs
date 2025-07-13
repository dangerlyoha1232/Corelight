using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class ServiceLocator
    {
        private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

        public static ServiceLocator Current { get; private set; }

        public static void Initialize()
        {
            Current = new ServiceLocator();
        }

        public void Register<T>(T service) where T : IService
        {
            string key = typeof(T).Name;

            if (_services.ContainsKey(key))
            {
                Debug.LogError("Service already exists!");
                return;
            }

            _services.Add(key, service);
        }

        public void UnRegister<T>(T service) where T : IService
        {
            string key = typeof(T).Name;

            if (!_services.ContainsKey(key))
            {
                Debug.LogError("Service not exists!");
                return;
            }

            _services.Remove(key);
        }

        public T Get<T>() where T : IService
        {
            string key = typeof(T).Name;

            if (!_services.ContainsKey(key))
            {
                Debug.LogError("Service not exists!");
                throw new InvalidOperationException();
            }

            return (T)_services[key];
        }
    }
}