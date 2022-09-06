using System.Collections.Generic;
using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using TrafficVolume.GUI;

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

        // private static UnityHook _unityHook;

        public static Dictionary<Transport, UICheckBox> CheckBoxDict { get; set; } 
            = new Dictionary<Transport, UICheckBox>();
        
        public static Dictionary<Transport, string> TransportNameDict { get; set; } 
            = new Dictionary<Transport, string>();

        // public static UIRadialChart TrafficChart { get; private set; }
        // public static UICheckBox OverlayCheckBox { get; private set; }
        
        // private static TrafficRoutesInfoViewPanel RoutesPanel => TrafficRoutesInfoViewPanel.instance;
        // private static UIComponent TrafficPanel => RoutesPanel.Find("ShowTransportTypes");
        
        public static void OnModEnabled()
        {
            AttachHarmony();
        }

        public static void OnLevelLoaded(LoadMode mode)
        {
            UnityHelper.InstantiateSingle(ref _globalVolumeGUI);
            UnityHelper.InstantiateSingle(ref _localVolumeGUI);

            // UnityHelper.InstantiateSingle(ref _unityHook);
            // _unityHook.UnityUpdate += OnUnityUpdate;

            // CreateOverlayCheckbox();
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
            
            // if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.P))
            // {
            //     CreateChart();
            // }
        }
        
        public static void OnSimulationUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            
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

        // private static void CreateChart()
        // {
        //     // does not work (yet)
        //     
        //     var routesPanel = RoutesPanel;
        //
        //     if (routesPanel == null)
        //     {
        //         Log.WriteInfo("Routes panel is null");
        //         return;
        //     }
        //     
        //     var subComponent = routesPanel.Find("ShowTransportTypes");
        //     
        //     if (subComponent == null)
        //     {
        //         Log.WriteInfo("Sub component is null");
        //         return;
        //     }
        //     
        //     var chart = subComponent.AddUIComponent<UIRadialChart>();
        //
        //     chart.gameObject.name = "TrafficVolumePieChart";
        //     
        //     // var chartGo = new GameObject("PieChart");
        //     // chartGo.transform.parent = subComponent.transform;
        //     // chartGo.transform.localPosition = Vector2.zero;
        //     //
        //     // Chart = chartGo.AddComponent<UIRadialChart>();
        //
        //     var types = (Transport[])Enum.GetValues(typeof(Transport));
        //     var typeCount = types.Length;
        //
        //     for (int i = 0; i < typeCount; i++)
        //     {
        //         chart.AddSlice();
        //
        //         var slice = TrafficChart.GetSlice(i);
        //
        //         if (slice == null)
        //         {
        //             Log.WriteInfo($"Slice {i} is null");
        //             return;
        //         }
        //         
        //         // todo actual transport type colors
        //         
        //         var r = Random.Range(0f, 1f);
        //         var g = Random.Range(0f, 1f);
        //         var b = Random.Range(0f, 1f);
        //
        //         slice.innerColor = new Color(r, g, b);
        //         
        //         var r2 = Random.Range(0f, 1f);
        //         var g2 = Random.Range(0f, 1f);
        //         var b2 = Random.Range(0f, 1f);
        //
        //         slice.outterColor = new Color(r2, g2, b2);
        //     }
        //
        //     TrafficChart = chart;
        //     
        //     Log.WriteInfo("Chart created");
        // }
    }
}