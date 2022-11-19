using System.Collections.Generic;
using ColossalFramework;
using TrafficVolume.Managers;

namespace TrafficVolume.Traffic
{
    public static class LocalTraffic
    {
        private static HashSet<InstanceID> _targets;

        public static Volume CountLocalVolume()
        {
            return CountLocalVolume(_targets);
        }
        
        public static Volume CountLocalVolume(HashSet<InstanceID> targets)
        {
            _targets = targets;
            
            var vehicleManager = Singleton<VehicleManager>.instance;
            var citizenManager = Singleton<CitizenManager>.instance;
            var netManager = Singleton<NetManager>.instance;
            var pathManager = Singleton<PathManager>.instance;
            var buildingManager = Singleton<BuildingManager>.instance;
            var districtManager = Singleton<DistrictManager>.instance;

            var volume = new Volume();

            if (targets == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: targets hash set is null");
                return volume;
            }

            volume.Prepare();
            
            for (int vehicleID = 0; vehicleID < Manager.VehicleMaxIndex; ++vehicleID)
            {
                var isOnSegment = Helper.IsVehicleOnSegment(vehicleID, targets, pathManager, netManager,
                    buildingManager, districtManager, vehicleManager);

                if (isOnSegment)
                {
                    volume.AddVehicle(vehicleID, vehicleManager);
                }
            }
            
            for (int citizenID = 0; citizenID < Manager.CitizenMaxIndex; ++citizenID)
            {
                var isOnSegment = Helper.IsCitizenOnSegment(citizenID, targets, pathManager, netManager,
                    buildingManager, districtManager, citizenManager);

                if (isOnSegment)
                {
                    volume.AddCitizen(citizenID, vehicleManager, citizenManager);
                }
            }

            return volume;
        }
    }
}