using System.Collections.Generic;
using ColossalFramework;

namespace TrafficVolume
{
    public static class LocalTraffic
    {
        public static readonly Volume Volume = new Volume();
        
        private static VehicleManager _vehicleManager;
        private static CitizenManager _citizenManager;
        private static NetManager _netManager;
        private static BuildingManager _buildingManager;
        private static DistrictManager _districtManager;
        private static PathManager _pathManager;

        public static void CountVolume(HashSet<InstanceID> targets)
        {
            _vehicleManager = Singleton<VehicleManager>.instance;
            _citizenManager = Singleton<CitizenManager>.instance;
            _netManager = Singleton<NetManager>.instance;
            _buildingManager = Singleton<BuildingManager>.instance;
            _districtManager = Singleton<DistrictManager>.instance;
            _pathManager = Singleton<PathManager>.instance;

            Volume.Clear();
            
            for (int vehicleID = 0; vehicleID < Manager.VehicleMaxIndex; ++vehicleID)
            {
                var isOnSegment = Helper.IsVehicleOnSegment(vehicleID, targets, _pathManager, _netManager,
                    _buildingManager, _districtManager, _vehicleManager);

                if (isOnSegment)
                {
                    Volume.AddVehicle(vehicleID, _vehicleManager);
                }
            }
            
            for (int citizenID = 0; citizenID < Manager.CitizenMaxIndex; ++citizenID)
            {
                var isOnSegment = Helper.IsCitizenOnSegment(citizenID, targets, _pathManager, _netManager,
                    _buildingManager, _districtManager, _citizenManager);

                if (isOnSegment)
                {
                    Volume.AddCitizen(citizenID, _vehicleManager, _citizenManager);
                }
            }
            
            Volume.MakeDictionary();
            
            // Manager.LocalVolumeGUI.Open();
            
            Display();
        }

        private static void Display()
        {
            foreach (var kvp in Volume.Dict)
            {
                var transport = kvp.Key;
                var count = kvp.Value;

                var checkBox = Manager.CheckBoxDict[transport];
                var transportName = Manager.TransportNameDict[transport];

                checkBox.text = $"{count} {transportName}";
            }
        }
        
        // public static void RegisterInstance(InstanceID instanceID)
        // {
        //     if (!_isCounting) return;
        //
        //     var vehicleId = instanceID.Vehicle;
        //     var citizenId = instanceID.CitizenInstance;
        //
        //     var isCitizen = citizenId != 0;
        //     var isVehicle = !isCitizen && vehicleId != 0;
        //
        //     if (isCitizen)
        //     {
        //         Volume.AddCitizen(citizenId, _vehicleManager, _citizenManager);
        //     }
        //
        //     if (isVehicle)
        //     {
        //         Volume.AddVehicle(vehicleId, _vehicleManager);
        //     }
        // }
    }
}