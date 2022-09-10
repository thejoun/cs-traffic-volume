using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficVolume
{
    public class Volume
    {
        public uint pedestrianCount = 0;
        public uint bicycleCount = 0;
        public uint residentialCount = 0;
        public uint industrialCount = 0;
        public uint postVehicleCount = 0;
        public uint cityServiceVehicleCount = 0;
        public uint publicTransportVehicleCount = 0;

        public Dictionary<Transport, uint> Dict => new Dictionary<Transport, uint>
        {
            {Transport.Pedestrian, pedestrianCount},
            {Transport.Cyclist, bicycleCount},
            {Transport.Private, residentialCount},
            {Transport.Public, publicTransportVehicleCount},
            {Transport.Truck, industrialCount},
            {Transport.Service, postVehicleCount + cityServiceVehicleCount}
        };

        public void Clear()
        {
            pedestrianCount = 0;
            bicycleCount = 0;
            residentialCount = 0;
            industrialCount = 0;
            postVehicleCount = 0;
            cityServiceVehicleCount = 0;
            publicTransportVehicleCount = 0;
        }

        public string Dump()
        {
            var builder = new StringBuilder();

            builder.Append($"Pedestrians: {pedestrianCount}\n");
            builder.Append($"Cyclists: {bicycleCount}\n");
            builder.Append($"Private vehicles: {residentialCount}\n");
            builder.Append($"Public transport: {publicTransportVehicleCount}\n");
            builder.Append($"Trucks: {industrialCount}\n");
            builder.Append($"City service: {postVehicleCount + cityServiceVehicleCount}\n");

            return builder.ToString();
        }

        public void AddVehicle(int index, VehicleManager vehicleManager)
        {
            var vehicle = vehicleManager.m_vehicles.m_buffer[index];

            if ((vehicle.m_flags & (Vehicle.Flags.Created | Vehicle.Flags.Deleted | Vehicle.Flags.WaitingPath)) ==
                Vehicle.Flags.Created)
            {
                AddVehicle(vehicle);
            }
        }

        public void AddCitizen(int index, VehicleManager vehicleManager, CitizenManager citizenManager)
        {
            var citizen = citizenManager.m_instances.m_buffer[index];

            if ((citizen.m_flags & (CitizenInstance.Flags.Created | CitizenInstance.Flags.Deleted |
                                    CitizenInstance.Flags.WaitingPath))
                == CitizenInstance.Flags.Created)
            {
                AddCitizen(citizen, citizenManager, vehicleManager);
            }
        }

        public bool TryGetTransportType(Vehicle vehicle, out Transport transport)
        {
            var vehicleInfo = vehicle.Info;
            var service = vehicleInfo.m_class.m_service;

            switch (service)
            {
                case ItemClass.Service.Residential:
                    transport = Transport.Private;
                    return true;
                case ItemClass.Service.Industrial:
                    transport = Transport.Truck;
                    return true;
                case ItemClass.Service.PublicTransport:
                    transport = Transport.Public;
                    return true;
                case ItemClass.Service.Fishing:
                    transport = default;
                    return false;
                default:
                    transport = Transport.Service;
                    return true;
            }
        }

        private void AddVehicle(Vehicle vehicle)
        {
            VehicleInfo vehicleInfo = vehicle.Info;

            ItemClass.Service service = vehicleInfo.m_class.m_service;

            switch (service)
            {
                case ItemClass.Service.Residential:
                    residentialCount++;
                    break;
                case ItemClass.Service.Industrial:
                    industrialCount++;
                    break;
                default:
                    switch (service - 19)
                    {
                        case ItemClass.Service.None:
                            var isPost = vehicleInfo.m_class.m_subService ==
                                         ItemClass.SubService.PublicTransportPost;
                            if (isPost)
                            {
                                postVehicleCount++;
                            }
                            else
                            {
                                publicTransportVehicleCount++;
                            }

                            break;
                        default:
                            if (service == ItemClass.Service.Fishing)
                            {
                                if (vehicleInfo.m_vehicleAI is FishingBoatAI)
                                {
                                    industrialCount++;
                                }
                                else
                                {
                                    publicTransportVehicleCount++;
                                }
                            }
                            else
                            {
                                cityServiceVehicleCount++;
                            }

                            break;
                    }

                    break;
            }
        }

        private void AddCitizen(CitizenInstance citizen, CitizenManager citizenManager,
            VehicleManager vehicleManager)
        {
            VehicleInfo.VehicleType vehicleType = VehicleInfo.VehicleType.None;

            uint citizenInstance = citizen.m_citizen;

            if (citizenInstance != 0U)
            {
                ushort vehicle = citizenManager.m_citizens.m_buffer[(uint) (IntPtr) citizenInstance].m_vehicle;

                if (vehicle != 0)
                {
                    VehicleInfo info = vehicleManager.m_vehicles.m_buffer[vehicle].Info;

                    if (info != null) vehicleType = info.m_vehicleType;
                }
            }

            switch (vehicleType)
            {
                case VehicleInfo.VehicleType.None:
                    pedestrianCount++;
                    break;
                case VehicleInfo.VehicleType.Bicycle:
                    bicycleCount++;
                    break;
            }
        }
    }
}