using System.Collections.Generic;
using ColossalFramework;
using TrafficVolume.Helpers;
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

            var vehicleCount = vehicleManager.m_vehicles.ItemCount();
            var citizenCount = citizenManager.m_instances.ItemCount();
            
            var volume = new Volume();

            volume.Prepare();

            for (int vehicleID = 0; vehicleID < vehicleCount; ++vehicleID)
            {
                var isOnSegment = VehicleHelper.IsVehicleOnSegment(vehicleID, targets, pathManager, netManager,
                    buildingManager, districtManager, vehicleManager);

                if (isOnSegment)
                {
                    volume.AddVehicle(vehicleID, vehicleManager);
                }
            }
            
            for (int citizenID = 0; citizenID < citizenCount; ++citizenID)
            {
                var isOnSegment = CitizenHelper.IsCitizenOnSegment(citizenID, targets, pathManager, netManager,
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