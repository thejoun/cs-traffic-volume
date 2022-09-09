using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

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
                Manager.Log.WriteLog("Local volume is null. Creating a new one");

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
                Manager.Log.WriteLog("CountLocalVolume: targets hash set is null");
                return;
            }

            if (_vehicleManager == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: vehicle manager is null");
                return;
            }

            if (_citizenManager == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: citizen manager is null");
                return;
            }

            if (_netManager == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: net manager is null");
                return;
            }

            if (_pathManager == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: path manager is null");
                return;
            }

            if (_buildingManager == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: building manager is null");
                return;
            }

            if (_districtManager == null)
            {
                Manager.Log.WriteLog("CountLocalVolume: district manager is null");
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
            DisplayChart();
        }

        private static void DisplayText()
        {
            var checkBoxDict = Manager.CheckBoxDict;

            var transportNameDict = Manager.TransportNameDict;

            foreach (var kvp in Volume.Dict)
            {
                var transport = kvp.Key;
                var count = kvp.Value;

                if (checkBoxDict.TryGetValue(transport, out var checkBox))
                {
                    if (transportNameDict.TryGetValue(transport, out var title))
                    {
                        var text = $"{count} {title}";
                        
                        checkBox.text = text;
                    }
                }
            }
        }

        private static void DisplayChart()
        {
            var countDict = Volume.Dict;
            var counts = countDict.Values.ToArray();
            
            var sum = counts.Sum(c => c);
            var percentages = counts.Select(c => 1f * c / sum).ToArray();
            
            var chart = Manager.Chart;
            
            chart.gameObject.SetActive(sum != 0);

            if (sum == 0)
            {
                return;
            }
            
            chart.transform.localPosition = Vector2.zero;
            
            var a = 0f;
            for (int index = 0; index < chart.sliceCount; index++)
            {
                var percentage = percentages[index];
                var slice = chart.GetSlice(index);

                // ensures that no weird clamps are applied later
                slice.startValue = 0f;
                slice.endValue = 1f;
                
                slice.startValue = Mathf.Clamp(a, 0f, 1f);
                slice.endValue = Mathf.Clamp(a + percentage, 0f, 1f);

                a += percentage;
            }
            chart.Invalidate();
        }
    }
}