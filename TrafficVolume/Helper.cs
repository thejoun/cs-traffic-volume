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
            Vehicle vehicle = vehicleManager.m_vehicles.m_buffer[index];
            VehicleInfo vehicleInfo = vehicle.Info;

            int pathPositionIndex = vehicle.m_pathPositionIndex;
            int startPositionIndex = pathPositionIndex != byte.MaxValue ? pathPositionIndex >> 1 : 0;

            uint pathUnitID = vehicle.m_path;

            bool flag1 = false;
            bool flag2 = false;

            int safetyCounter = 0;

            while (pathUnitID != 0U && !flag1 && !flag2)
            {
                PathUnit pathUnit = pathManager.m_pathUnits.m_buffer[(uint) (IntPtr) pathUnitID];

                int positionCount = pathUnit.m_positionCount;

                for (int positionIndex = startPositionIndex; positionIndex < positionCount; ++positionIndex)
                {
                    PathUnit.Position position = pathUnit.GetPosition(positionIndex);

                    InstanceID segmentInstance = new InstanceID() {NetSegment = position.m_segment};

                    if (targets.Contains(segmentInstance))
                    {
                        NetSegment segment = netManager.m_segments.m_buffer[position.m_segment];

                        if (segment.m_modifiedIndex < pathUnit.m_buildIndex)
                        {
                            NetInfo netInfo = netManager.m_segments.m_buffer[position.m_segment].Info;

                            var hasLanes = netInfo.m_lanes != null;

                            if (hasLanes)
                            {
                                var laneOk = position.m_lane < netInfo.m_lanes.Length;

                                if (laneOk)
                                {
                                    var lane = netInfo.m_lanes[position.m_lane];

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
                    CODebugBase<LogChannel>.Error(LogChannel.Core,
                        "Invalid list detected!\n" + System.Environment.StackTrace);
                    break;
                }
            }

            InstanceID targetId = vehicleInfo.m_vehicleAI.GetTargetID((ushort) index, ref vehicle);

            bool flag3 = flag1 | targets.Contains(targetId);

            var targetIsBuilding = targetId.Building != 0;
            var targetIsNetNode = targetId.NetNode != 0;

            if (targetIsBuilding)
            {
                Vector3 position = buildingManager.m_buildings.m_buffer[targetId.Building].m_position;
                InstanceID empty = InstanceID.Empty;
                empty.District = districtManager.GetDistrict(position);
                bool flag4 = flag3 | targets.Contains(empty);
                empty.Park = districtManager.GetPark(position);
                flag3 = flag4 | targets.Contains(empty);
            }

            if (targetIsNetNode)
            {
                Vector3 position = netManager.m_nodes.m_buffer[targetId.NetNode].m_position;
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
            //     empty.Vehicle = (ushort) index;
            //     
            //     this.AddInstance(empty);
            // }
        }

        public static bool IsCitizenOnSegment(int index1, HashSet<InstanceID> targets, PathManager instance1,
            NetManager instance2, BuildingManager instance3, DistrictManager instance4,
            CitizenManager instance6)
        {
            bool flag1 = false;
            int pathPositionIndex = (int) instance6.m_instances.m_buffer[index1].m_pathPositionIndex;
            int num1 = pathPositionIndex != (int) byte.MaxValue ? pathPositionIndex >> 1 : 0;
            uint num2 = instance6.m_instances.m_buffer[index1].m_path;
            bool flag2 = false;
            int num3 = 0;
            while (num2 != 0U && !flag1 && !flag2)
            {
                int positionCount = (int) instance1.m_pathUnits.m_buffer[(uint) (IntPtr) num2].m_positionCount;
                for (int index2 = num1; index2 < positionCount; ++index2)
                {
                    PathUnit.Position position =
                        instance1.m_pathUnits.m_buffer[(uint) (IntPtr) num2].GetPosition(index2);
                    InstanceID empty = InstanceID.Empty;
                    empty.NetSegment = position.m_segment;
                    if (targets.Contains(empty))
                    {
                        if (instance2.m_segments.m_buffer[(int) position.m_segment].m_modifiedIndex <
                            instance1.m_pathUnits.m_buffer[(uint) (IntPtr) num2].m_buildIndex)
                        {
                            NetInfo info = instance2.m_segments.m_buffer[(int) position.m_segment].Info;
                            if (info.m_lanes != null && (int) position.m_lane < info.m_lanes.Length &&
                                (info.m_lanes[(int) position.m_lane].m_laneType == NetInfo.LaneType.Pedestrian ||
                                 info.m_lanes[(int) position.m_lane].m_laneType == NetInfo.LaneType.Vehicle &&
                                 info.m_lanes[(int) position.m_lane].m_vehicleType == VehicleInfo.VehicleType.Bicycle))
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
                num2 = instance1.m_pathUnits.m_buffer[(uint) (IntPtr) num2].m_nextPathUnit;
                if (++num3 >= 262144)
                {
                    CODebugBase<LogChannel>.Error(LogChannel.Core,
                        "Invalid list detected!\n" + System.Environment.StackTrace);
                    break;
                }
            }

            InstanceID targetId = instance6.m_instances.m_buffer[index1].Info.m_citizenAI
                .GetTargetID((ushort) index1, ref instance6.m_instances.m_buffer[index1]);
            bool flag3 = flag1 | targets.Contains(targetId);
            if (targetId.Building != (ushort) 0)
            {
                Vector3 position = instance3.m_buildings.m_buffer[(int) targetId.Building].m_position;
                InstanceID empty = InstanceID.Empty;
                empty.District = instance4.GetDistrict(position);
                bool flag4 = flag3 | targets.Contains(empty);
                empty.Park = instance4.GetPark(position);
                flag3 = flag4 | targets.Contains(empty);
            }

            if (targetId.NetNode != (ushort) 0)
            {
                Vector3 position = instance2.m_nodes.m_buffer[(int) targetId.NetNode].m_position;
                InstanceID empty = InstanceID.Empty;
                empty.District = instance4.GetDistrict(position);
                bool flag4 = flag3 | targets.Contains(empty);
                empty.Park = instance4.GetPark(position);
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