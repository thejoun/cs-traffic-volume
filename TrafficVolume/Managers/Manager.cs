using System;
using TrafficVolume.TempGUI;
using TrafficVolume.Traffic;

#pragma warning disable 0649

namespace TrafficVolume.Managers
{
    public static class Manager
    {
        public const int VehicleMaxIndex = 256 * 64;
        public const int CitizenMaxIndex = 256 * 256;

        private static Log _log;
        private static GlobalVolumeGUI _globalVolumeGUI;
        private static float _refreshTimer;

        public static Log Log => _log ?? (_log = new Log(ModInfo.HarmonyModID));

        private static bool AutoRefreshEnabled => true;
        private static float RefreshInterval => 1f;
        
        public static event Action Refresh;
        
        public static void OnLevelLoaded()
        {
            UnityHelper.InstantiateSingle(ref _globalVolumeGUI);

            ResetRefreshTimer();
        }

        public static void OnSimulationUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            _refreshTimer += realTimeDelta;

            if (_refreshTimer > RefreshInterval)
            {
                OnRefreshTimerGoal();
                
                _refreshTimer -= RefreshInterval;
            }
        }

        public static void ResetRefreshTimer()
        {
            _refreshTimer = 0f;
        }

        private static void OnRefreshTimerGoal()
        {
            if (AutoRefreshEnabled && UIManager.IsTrafficPanelOpen)
            {
                Log.WriteLog($"Auto-refreshing local volume");
                
                var volume = LocalTraffic.CountLocalVolume();
                UIManager.DisplayVolume(volume);
            }
            else
            {
                Log.WriteLog($"No refresh");
            }

            Refresh?.Invoke();
        }
    }
}