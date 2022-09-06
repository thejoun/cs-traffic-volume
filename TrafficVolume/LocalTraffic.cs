using System.Collections.Generic;
using ColossalFramework;

namespace TrafficVolume
{
    public static class LocalTraffic
    {
        public static Volume Volume = new Volume();
        
        private static VehicleManager _vehicleManager;
        private static CitizenManager _citizenManager;
        private static NetManager _netManager;
        private static BuildingManager _buildingManager;
        private static DistrictManager _districtManager;
        private static PathManager _pathManager;

        public static void CountLocalVolume(HashSet<InstanceID> targets)
        {
            // Manager.Log.WriteInfo("Count local volume");

            if (Volume == null)
            {
                Manager.Log.WriteInfo("Local volume is null. Creating a new one");

                Volume = new Volume();
            }
            
            _vehicleManager = Singleton<VehicleManager>.instance;
            _citizenManager = Singleton<CitizenManager>.instance;
            _netManager = Singleton<NetManager>.instance;
            _pathManager = Singleton<PathManager>.instance;
            _buildingManager = Singleton<BuildingManager>.instance;
            _districtManager = Singleton<DistrictManager>.instance;

            if (targets == null)
            {
                Manager.Log.WriteInfo("CountLocalVolume: targets hash set is null");
                return;
            }

            if (_vehicleManager == null)
            {
                Manager.Log.WriteInfo("CountLocalVolume: vehicle manager is null");
                return;
            }

            if (_citizenManager == null)
            {
                Manager.Log.WriteInfo("CountLocalVolume: citizen manager is null");
                return;
            }

            if (_netManager == null)
            {
                Manager.Log.WriteInfo("CountLocalVolume: net manager is null");
                return;
            }

            if (_pathManager == null)
            {
                Manager.Log.WriteInfo("CountLocalVolume: path manager is null");
                return;
            }

            if (_buildingManager == null)
            {
                Manager.Log.WriteInfo("CountLocalVolume: building manager is null");
                return;
            }

            if (_districtManager == null)
            {
                Manager.Log.WriteInfo("CountLocalVolume: district manager is null");
                return;
            }

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
            
            DisplayText();
            
            // DisplayChart();
        }

        private static void DisplayText()
        {
            var checkBoxDict = Manager.CheckBoxDict;

            if (checkBoxDict == null)
            {
                Manager.Log.WriteInfo("Checkbox dict is null");
                return;
            }

            var transportNameDict = Manager.TransportNameDict;
            
            if (transportNameDict == null)
            {
                Manager.Log.WriteInfo("Transport name dict is null");
                return;
            }

            if (Volume == null)
            {
                Manager.Log.WriteInfo("Local Volume is null");
                return;
            }
            
            foreach (var kvp in Volume.Dict)
            {
                var transport = kvp.Key;
                var count = kvp.Value;

                if (checkBoxDict.TryGetValue(transport, out var checkBox))
                {
                    if (!checkBox)
                    {
                        Manager.Log.WriteInfo("Checkbox is null");
                        continue;
                    }
                    
                    if (transportNameDict.TryGetValue(transport, out var title))
                    {
                        var text = $"{count} {title}";
                        
                        // Manager.Log.WriteInfo($"Displaying text {text}");

                        checkBox.text = text;
                    }
                    else
                    {
                        Manager.Log.WriteInfo($"No title for {transport} in dict");
                        continue;
                    }
                }
                else
                {
                    Manager.Log.WriteInfo($"No checkbox for {transport} in dict");
                    continue;
                }
            }
        }

        // private static void DisplayChart()
        // {
        //     var countDict = Volume.Dict;
        //     var counts = countDict.Values.ToArray();
        //     var sum = counts.Sum(c => c);
        //     var percentages = counts.Select(c => 1f * c / sum).ToArray();
        //
        //     Manager.Chart.SetValues(percentages);
        // }
    }
}