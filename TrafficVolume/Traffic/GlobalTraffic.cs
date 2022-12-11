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

            var vehicleCount = vehicleManager.m_vehicles.ItemCount();
            var citizenCount = citizenManager.m_instances.ItemCount();
            
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