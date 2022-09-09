using System;
using System.Collections.Generic;
using ColossalFramework;
using UnityEngine;

namespace TrafficVolume
{
    public static class Helper
    {
        public static bool IsVehicleOnSegment(int index, HashSet<InstanceID> targets, PathManager pathManager,
            NetManager netManager, BuildingManager buildingManager, DistrictManager districtManager,
            VehicleManager vehicleManager)
        {
            Vehicle vehicle = vehicleManager.m_vehicles?.m_buffer?[index] ?? default;

            int pathPositionIndex = vehicle.m_pathPositionIndex;
            int startPositionIndex = pathPositionIndex != byte.MaxValue ? pathPositionIndex >> 1 : 0;

            uint pathUnitID = vehicle.m_path;

            bool flag1 = false;
            bool flag2 = false;

            int safetyCounter = 0;

            while (pathUnitID != 0U && !flag1 && !flag2)
            {
                PathUnit pathUnit = pathManager.m_pathUnits?.m_buffer?[(uint) (IntPtr) pathUnitID] ?? default;

                int positionCount = pathUnit.m_positionCount;

                for (int positionIndex = startPositionIndex; positionIndex < positionCount; ++positionIndex)
                {
                    PathUnit.Position position = pathUnit.GetPosition(positionIndex);

                    InstanceID segmentInstance = new InstanceID() {NetSegment = position.m_segment};

                    if (targets.Contains(segmentInstance))
                    {
                        NetSegment segment = netManager.m_segments?.m_buffer?[position.m_segment] ?? default;

                        if (segment.m_modifiedIndex < pathUnit.m_buildIndex)
                        {
                            NetInfo netInfo = segment.Info;

                            if (netInfo == null)
                            {
                                Manager.Log.WriteLog("IsVehicleOnSegment: NetInfo is null");
                                return false;
                            }

                            var hasLanes = netInfo.m_lanes != null;

                            if (hasLanes)
                            {
                                var laneOk = position.m_lane < netInfo.m_lanes.Length;

                                if (laneOk)
                                {
                                    var lane = netInfo.m_lanes[position.m_lane];

                                    if (lane == null)
                                    {
                                        Manager.Log.WriteLog("Lane is null");
                                        return false;
                                    }
                                    
                                    var isVehicleLane = (lane.m_laneType &
                                                         (NetInfo.LaneType.Vehicle | NetInfo.LaneType.TransportVehicle))
                                                        != NetInfo.LaneType.None;

                                    if (isVehicleLane)
                                    {
                                        flag1 = true;
                                        break;
                                    }
                                }
                            }
                        }

                        flag2 = true;
                        break;
                    }
                }

                startPositionIndex = 0;

                pathUnitID = pathUnit.m_nextPathUnit;

                if (++safetyCounter >= 262144)
                {
                    // CODebugBase<LogChannel>.Error(LogChannel.Core,
                    //     "Invalid list detected!\n" + System.Environment.StackTrace);
                    
                    Manager.Log.WriteLog("Invalid list detected\n" + System.Environment.StackTrace);
                    
                    break;
                }
            }

            VehicleInfo vehicleInfo = vehicle.Info;

            // important catch!
            if (vehicleInfo == null)
            {
                // Manager.Log.WriteInfo("IsVehicleOnSegment: VehicleInfo is null");
                return flag1;
            }

            var vehicleAI = vehicleInfo.m_vehicleAI;

            if (!vehicleAI)
            {
                Manager.Log.WriteLog("VehicleAI is null");
                return flag1;
            }
            
            InstanceID targetId = vehicleAI.GetTargetID((ushort) index, ref vehicle);

            bool flag3 = flag1 | targets.Contains(targetId);

            var targetIsBuilding = targetId.Building != 0;
            var targetIsNetNode = targetId.NetNode != 0;

            if (targetIsBuilding)
            {
                var building = buildingManager.m_buildings?.m_buffer?[targetId.Building] ?? default;
                var position = building.m_position;
                
                var empty = InstanceID.Empty;
                empty.District = districtManager.GetDistrict(position);
                bool flag4 = flag3 | targets.Contains(empty);
                empty.Park = districtManager.GetPark(position);
                flag3 = flag4 | targets.Contains(empty);
            }

            if (targetIsNetNode)
            {
                var netNode = netManager.m_nodes?.m_buffer?[targetId.NetNode] ?? default;
                var position = netNode.m_position;
                
                var empty = InstanceID.Empty;
                empty.District = districtManager.GetDistrict(position);
                bool flag4 = flag3 | targets.Contains(empty);
                empty.Park = districtManager.GetPark(position);
                flag3 = flag4 | targets.Contains(empty);
            }

            return flag3;

            // if (flag3)
            // {
            //     InstanceID empty = InstanceID.Empty;
            //     empty.Vehicle = (ushort) index;
            //     
            //     this.AddInstance(empty);
            // }
        }

        public static bool IsCitizenOnSegment(int index, HashSet<InstanceID> targets, PathManager pathManager,
            NetManager netManager, BuildingManager buildingManager, DistrictManager districtManager,
            CitizenManager citizenManager)
        {
            bool flag1 = false;
            
            CitizenInstance citizenInstance = citizenManager.m_instances?.m_buffer?[index] ?? default;
            
            int pathPositionIndex = citizenInstance.m_pathPositionIndex;
            
            int num1 = pathPositionIndex != byte.MaxValue ? pathPositionIndex >> 1 : 0;
            uint num2 = citizenInstance.m_path;
            int num3 = 0;

            bool flag2 = false;


            while (num2 != 0U && !flag1 && !flag2)
            {
                var pathUnit = pathManager.m_pathUnits?.m_buffer?[(uint) (IntPtr) num2] ?? default;
                int positionCount = pathUnit.m_positionCount;
                
                for (int index2 = num1; index2 < positionCount; ++index2)
                {
                    PathUnit.Position position = pathUnit.GetPosition(index2);
                    
                    InstanceID empty = InstanceID.Empty;
                    empty.NetSegment = position.m_segment;
                    
                    if (targets.Contains(empty))
                    {
                        var netSegment = netManager.m_segments?.m_buffer?[position.m_segment] ?? default;
                        
                        if (netSegment.m_modifiedIndex < pathUnit.m_buildIndex)
                        {
                            NetInfo info = netSegment.Info;

                            var hasLanes = info.m_lanes != null;
                            var lane = position.m_lane;
                            var mLane = hasLanes ? info.m_lanes[lane] : default;
                            
                            if (hasLanes && (int) lane < info.m_lanes.Length &&
                                (mLane.m_laneType == NetInfo.LaneType.Pedestrian ||
                                 mLane.m_laneType == NetInfo.LaneType.Vehicle &&
                                 mLane.m_vehicleType == VehicleInfo.VehicleType.Bicycle))
                            {
                                flag1 = true;
                                break;
                            }
                        }

                        flag2 = true;
                        break;
                    }
                }

                num1 = 0;
                num2 = pathUnit.m_nextPathUnit;
                if (++num3 >= 262144)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core,
                        "Invalid list detected\n" + System.Environment.StackTrace);
                    break;
                }
            }

            var citizenInfo = citizenInstance.Info;

            if (citizenInfo == null)
            {
                Manager.Log.WriteLog("Citizen info is null");
                return flag1;
            }
            
            var citizenAI = citizenInfo.m_citizenAI;

            if (!citizenAI)
            {
                Manager.Log.WriteLog("Citizen AI is null");
                return flag1;
            }
            
            InstanceID targetId = citizenAI.GetTargetID((ushort) index, ref citizenManager.m_instances.m_buffer[index]);
            
            bool flag3 = flag1 | targets.Contains(targetId);
            if (targetId.Building != (ushort) 0)
            {
                Vector3 position = buildingManager.m_buildings.m_buffer[(int) targetId.Building].m_position;
                InstanceID empty = InstanceID.Empty;
                empty.District = districtManager.GetDistrict(position);
                bool flag4 = flag3 | targets.Contains(empty);
                empty.Park = districtManager.GetPark(position);
                flag3 = flag4 | targets.Contains(empty);
            }

            if (targetId.NetNode != (ushort) 0)
            {
                Vector3 position = netManager.m_nodes.m_buffer[(int) targetId.NetNode].m_position;
                InstanceID empty = InstanceID.Empty;
                empty.District = districtManager.GetDistrict(position);
                bool flag4 = flag3 | targets.Contains(empty);
                empty.Park = districtManager.GetPark(position);
                flag3 = flag4 | targets.Contains(empty);
            }

            return flag3;

            // if (flag3)
            // {
            //     InstanceID empty = InstanceID.Empty;
            //     empty.CitizenInstance = (ushort) index1;
            //
            //     this.AddInstance(empty);
            //     if (this.m_neededPathCount >= 100)
            //         break;
            // }
        }
    }
}