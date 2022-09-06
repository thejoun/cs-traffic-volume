using UnityEngine;

namespace TrafficVolume
{
    public static class UnityHelper
    {
        public static void InstantiateSingle<T>(ref T component, bool dontDestroy = false)
            where T : Component
        {
            if (component)
            {
                return;
            }

            component = Instantiate<T>(dontDestroy);
        }

        public static T Instantiate<T>(bool dontDestroy = false)
            where T : Component
        {
            var typeName = typeof(T).Name;

            var go = Object.Instantiate(new GameObject());
            go.name = typeName;
            var component = go.AddComponent<T>();

            if (dontDestroy)
            {
                Object.DontDestroyOnLoad(go);
            }
            
            // Log.WriteInfo($"{typeName} instantiated");

            return component;
        }
    }
}