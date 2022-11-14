using System.Collections.Generic;
using ColossalFramework;

namespace TrafficVolume
{
    public static class LocalTraffic
    {
        public static readonly Volume Volume = new Volume();

        public static void CountLocalVolume(HashSet<InstanceID> targets)
        {
            var vehicleManager = Singleton<VehicleManager>.instance;
            var citizenManager = Singleton<CitizenManager>.instance;
            var netManager = Singleton<NetManager>.instance;
            var pathManager = Singleton<PathManager>.instance;
            var buildingManager = Singleton<BuildingManager>.instance;
            var districtManager = Singleton<DistrictManager>.instance;

            if (targets == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: targets hash set is null");
                return;
            }

            Volume.Prepare();
            
            for (int vehicleID = 0; vehicleID < Manager.VehicleMaxIndex; ++vehicleID)
            {
                var isOnSegment = Helper.IsVehicleOnSegment(vehicleID, targets, pathManager, netManager,
                    buildingManager, districtManager, vehicleManager);

                if (isOnSegment)
                {
                    Volume.AddVehicle(vehicleID, vehicleManager);
                }
            }
            
            for (int citizenID = 0; citizenID < Manager.CitizenMaxIndex; ++citizenID)
            {
                var isOnSegment = Helper.IsCitizenOnSegment(citizenID, targets, pathManager, netManager,
                    buildingManager, districtManager, citizenManager);

                if (isOnSegment)
                {
                    Volume.AddCitizen(citizenID, vehicleManager, citizenManager);
                }
            }

            UIManager.DisplayVolume(Volume);
        }
    }
}