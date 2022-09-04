using ColossalFramework;

namespace TrafficVolume
{
    public static class GlobalTraffic
    {
        public static readonly Volume Volume = new Volume(); 
        
        public static void CountVolume()
        {
            var vehicleManager = Singleton<VehicleManager>.instance;
            var citizenManager = Singleton<CitizenManager>.instance;

            Volume.Clear();
            
            for (int vehicleID = 0; vehicleID < Manager.VehicleMaxIndex; ++vehicleID)
            {
                Volume.AddVehicle(vehicleID, vehicleManager);
            }
            
            for (int citizenID = 0; citizenID < Manager.CitizenMaxIndex; ++citizenID)
            {
                Volume.AddCitizen(citizenID, vehicleManager, citizenManager);
            }
        }
    }
}