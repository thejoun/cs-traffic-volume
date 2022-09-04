using System.Collections.Generic;
using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using TrafficVolume.GUI;
using UnityEngine;

#pragma warning disable 0649

namespace TrafficVolume
{
    public static class Manager
    {
        private const string HarmonyModID = "com.jonu.trafficvolume";
        
        public const int VehicleMaxIndex = 256 * 64;
        public const int CitizenMaxIndex = 256 * 256;

        public static Log Log { get; private set; }

        private static UnityHook _unityHook;
        private static GlobalVolumeGUI _globalVolumeGUI;
        private static LocalVolumeGUI _localVolumeGUI;
        
        public static Dictionary<Transport, UICheckBox> CheckBoxDict { get; set; } 
            = new Dictionary<Transport, UICheckBox>();
        
        public static Dictionary<Transport, string> TransportNameDict { get; set; } 
            = new Dictionary<Transport, string>();

        public static UIRadialChart Chart { get; private set; }
        
        private static TrafficRoutesInfoViewPanel RoutesPanel => TrafficRoutesInfoViewPanel.instance;
        
        public static void OnModEnabled()
        {
            Log = new Log(HarmonyModID);
            
            AttachHarmony();
        }

        public static void OnLevelLoaded(LoadMode mode)
        {
            InstantiateSingle(ref _unityHook);
            _unityHook.UnityUpdate += OnUnityUpdate;
            
            InstantiateSingle(ref _globalVolumeGUI);
            InstantiateSingle(ref _localVolumeGUI);
        }

        private static void OnUnityUpdate()
        {
            if (Input.GetKey(KeyCode.H) && Input.GetKeyDown(KeyCode.D))
            {
                UnityDump.DumpHierarchy();
            }   
        }
        
        public static void OnSimulationUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            
        }

        private static void CreateChart()
        {
            // todo
            
            var panel = RoutesPanel;
        }

        private static void AttachHarmony()
        {
            // Harmony.DEBUG = true;
            
            // Log.WriteInfo("Creating Harmony...");
            
            var harmony = new Harmony(HarmonyModID);

            // Log.WriteInfo("Patching...");
            
            harmony.PatchAll();

            // Log.WriteInfo("Patched.");
        }

        private static void InstantiateSingle<T>(ref T component, bool dontDestroy = false)
            where T : Component
        {
            if (component)
            {
                return;
            }

            component = Instantiate<T>(dontDestroy);
        }

        private static T Instantiate<T>(bool dontDestroy = false)
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
            
            Log.WriteInfo($"{typeName} instantiated");

            return component;
        }
    }
}