using ColossalFramework;
using TrafficVolume.Managers;

namespace TrafficVolume.Traffic
{
    public static class GlobalTraffic
    {
        public static Volume CountVolume()
        {
            var vehicleManager = Singleton<VehicleManager>.instance;
            var citizenManager = Singleton<CitizenManager>.instance;

            var vehicleCount = vehicleManager.m_vehicles.m_size;
            var citizenCount = citizenManager.m_instances.m_size;
            
            var volume = new Volume();
            
            volume.Prepare();
            
            for (int vehicleID = 0; vehicleID < vehicleCount; ++vehicleID)
            {
                volume.AddVehicle(vehicleID, vehicleManager);
            }
            
            for (int citizenID = 0; citizenID < citizenCount; ++citizenID)
            {
                volume.AddCitizen(citizenID, vehicleManager, citizenManager);
            }

            return volume;
        }
    }
}