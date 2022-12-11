using System;
using System.Collections.Generic;
using TrafficVolume.Extensions;
using TrafficVolume.Managers;

namespace TrafficVolume.Helpers
{
    public static class VehicleHelper
    {
        public static bool IsVehicleOnSegment(int vehicleIndex, HashSet<InstanceID> targets, PathManager pathManager,
            NetManager netManager, BuildingManager buildingManager, DistrictManager districtManager,
            VehicleManager vehicleManager)
        {
            var vehicleArray = vehicleManager.m_vehicles;
            var pathUnitArray = pathManager.m_pathUnits;
            var segmentArray = netManager.m_segments;
            var buildingArray = buildingManager.m_buildings;
            var nodeArray = netManager.m_nodes;

            if (vehicleArray.TryGetValue(vehicleIndex, out var vehicle))
            {
                int pathPositionIndex = vehicle.m_pathPositionIndex;
                int startPositionIndex = pathPositionIndex != byte.MaxValue ? pathPositionIndex >> 1 : 0;

                uint pathUnitID = vehicle.m_path;

                bool flag1 = false;
                bool flag2 = false;

                int safetyCounter = 0;

                while (pathUnitID != 0U && !flag1 && !flag2)
                {
                    var pathUnitIndex = (uint) (IntPtr) pathUnitID;

                    if (pathUnitArray.TryGetValue((int) pathUnitIndex, out var pathUnit))
                    {
                        int positionCount = pathUnit.m_positionCount;

                        for (int positionIndex = startPositionIndex; positionIndex < positionCount; ++positionIndex)
                        {
                            PathUnit.Position position = pathUnit.GetPosition(positionIndex);

                            InstanceID segmentInstance = new InstanceID() {NetSegment = position.m_segment};

                            if (targets.Contains(segmentInstance))
                            {
                                var segmentIndex = position.m_segment;

                                if (segmentArray.TryGetValue(segmentIndex, out var segment))
                                {
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
                                                                     (NetInfo.LaneType.Vehicle |
                                                                      NetInfo.LaneType.TransportVehicle))
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
                        }

                        startPositionIndex = 0;

                        pathUnitID = pathUnit.m_nextPathUnit;

                        if (++safetyCounter >= 262144)
                        {
                            Manager.Log.WriteLog("Invalid list detected\n" + Environment.StackTrace);

                            break;
                        }
                    }
                }

                VehicleInfo vehicleInfo = vehicle.Info;

                // important catch!
                if (vehicleInfo == null)
                {
                    return flag1;
                }

                var vehicleAI = vehicleInfo.m_vehicleAI;

                if (!vehicleAI)
                {
                    Manager.Log.WriteLog("VehicleAI is null");
                    return flag1;
                }

                InstanceID targetId = vehicleAI.GetTargetID((ushort) vehicleIndex, ref vehicle);

                bool flag3 = flag1 | targets.Contains(targetId);

                var targetIsBuilding = targetId.Building != 0;
                var targetIsNetNode = targetId.NetNode != 0;

                if (targetIsBuilding)
                {
                    if (buildingArray.TryGetValue(targetId.Building, out var building))
                    {
                        var position = building.m_position;

                        var empty = InstanceID.Empty;
                        empty.District = districtManager.GetDistrict(position);
                        bool flag4 = flag3 | targets.Contains(empty);
                        empty.Park = districtManager.GetPark(position);
                        flag3 = flag4 | targets.Contains(empty);
                    }
                }

                if (targetIsNetNode)
                {
                    if (nodeArray.TryGetValue(targetId.NetNode, out var netNode))
                    {
                        var position = netNode.m_position;

                        var empty = InstanceID.Empty;
                        empty.District = districtManager.GetDistrict(position);
                        bool flag4 = flag3 | targets.Contains(empty);
                        empty.Park = districtManager.GetPark(position);
                        flag3 = flag4 | targets.Contains(empty);
                    }
                }

                return flag3;
            }

            return false;
        }
    }
}