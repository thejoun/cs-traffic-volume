using ICities;
using TrafficVolume.TempGUI;

#pragma warning disable 0649

namespace TrafficVolume
{
    public static class Manager
    {
        public const int VehicleMaxIndex = 256 * 64;
        public const int CitizenMaxIndex = 256 * 256;

        private static Log _log;
        
        public static Log Log => _log ?? (_log = new Log(ModInfo.HarmonyModID));

        private static GlobalVolumeGUI _globalVolumeGUI;
        
        public static void OnLevelLoaded()
        {
            UnityHelper.InstantiateSingle(ref _globalVolumeGUI);
        }

        public static void OnSimulationUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            
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
    }
}