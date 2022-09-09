using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using Template.Extensions;
using TrafficVolume.GUI;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable 0649

namespace TrafficVolume
{
    public static class Manager
    {
        private const string HarmonyModID = "trafficvolume";

        public const string VersionId = "6 Sep";
        
        public const int VehicleMaxIndex = 256 * 64;
        public const int CitizenMaxIndex = 256 * 256;

        private static Log _log;
        
        public static Log Log => _log ?? (_log = new Log(HarmonyModID));

        private static GlobalVolumeGUI _globalVolumeGUI;
        private static LocalVolumeGUI _localVolumeGUI;

        private static UnityHook _unityHook;

        public static Dictionary<Transport, UICheckBox> CheckBoxDict { get; set; } 
            = new Dictionary<Transport, UICheckBox>();
        
        public static Dictionary<Transport, string> TransportNameDict { get; set; } 
            = new Dictionary<Transport, string>();

        public static UIRadialChart Chart { get; private set; }
        
        private static TrafficRoutesInfoViewPanel RoutesPanel => TrafficRoutesInfoViewPanel.instance;
        
        public static void OnModEnabled()
        {
            AttachHarmony();
        }

        public static void OnLevelLoaded(LoadMode mode)
        {
            UnityHelper.InstantiateSingle(ref _globalVolumeGUI);
            UnityHelper.InstantiateSingle(ref _localVolumeGUI);

            UnityHelper.InstantiateSingle(ref _unityHook);
            _unityHook.UnityUpdate += OnUnityUpdate;

            CreateChart();
        }

        private static void OnUnityUpdate()
        {
            // if (Input.GetKey(KeyCode.H) && Input.GetKeyDown(KeyCode.D))
            // {
            //     UnityDump.DumpHierarchy();
            // }
            
            // if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.L))
            // {
            //     CreateOverlayCheckbox();
            // }
            
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.P))
            {
                CreateChart();
            }
        }
        
        public static void OnSimulationUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            
        }
        
        private static void AttachHarmony()
        {
            // Harmony.DEBUG = true;

            var harmony = new Harmony(HarmonyModID);
            harmony.PatchAll();
        }

        // private static void CreateOverlayCheckbox()
        // {
        //     var parent = TrafficPanel;
        //
        //     parent.size = parent.size + new Vector2(0, 20);
        //     
        //     var checkbox = UIUtils.CreateCheckBox(parent);
        //
        //     checkbox.isChecked = true;
        //
        //     checkbox.eventCheckChanged += OnCheckChanged;
        // }

        // private static void OnCheckChanged(UIComponent ui, bool check)
        // {
        //     Log.WriteInfo($"Check: {check}");
        // }

        private static void CreateChart()
        {
            var routesPanel = RoutesPanel;
        
            if (routesPanel == null)
            {
                Log.Write("Routes panel is null");
                return;
            }

            var transportComponent = routesPanel.Find("ShowTransportTypes");
            
            if (transportComponent == null)
            {
                Log.Write("Transport component is null");
                return;
            }

            var transportPanel = transportComponent.gameObject.GetComponent<UIPanel>();
            
            if (transportPanel == null)
            {
                Log.Write("Transport panel is null");
                return;
            }
            
            transportPanel.autoLayout = false;
            
            var chart = transportComponent.AddUIComponent<UIRadialChart>();
            chart.size = new Vector2(100, 100);
            chart.spriteName = "PieChartWhiteBg";

            chart.transform.localPosition = Vector2.zero;
            chart.arbitraryPivotOffset = new Vector2(270, -38);

            chart.gameObject.name = "TrafficVolumePieChart";

            var types = (Transport[])Enum.GetValues(typeof(Transport));
            var typeCount = types.Length;

            var typeColors = Singleton<InfoManager>.instance.m_properties.m_routeColors;
            
            for (int i = 0; i < typeCount; i++)
            {
                chart.AddSlice();

                var slice = chart.GetSlice(i);
        
                if (slice == null)
                {
                    Log.Write($"Slice {i} is null");
                    return;
                }

                slice.endValue = 0f;
                
                var color = typeColors[i];
                
                // cyclists - more blue
                if (i == 1) color = color.ModifyHSV(0.05f, 0f, 0f);

                slice.innerColor = color;
                slice.outterColor = color;//.ModifyHSV(0.02f, 0f, -0.1f);

                // var r = Random.Range(0f, 1f);
                // var g = Random.Range(0f, 1f);
                // var b = Random.Range(0f, 1f);
                //
                // slice.innerColor = new Color(r, g, b);
                //
                // var r2 = Random.Range(0f, 1f);
                // var g2 = Random.Range(0f, 1f);
                // var b2 = Random.Range(0f, 1f);
                //
                // slice.outterColor = new Color(r2, g2, b2);
            }
        
            Chart = chart;
        }
    }
}