using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficVolume
{
    public class Volume : Dictionary<TransportType, uint>
    {
        public void Prepare()
        {
            Clear();
            
            var types = (TransportType[]) Enum.GetValues(typeof(TransportType));

            foreach (var type in types)
            {
                Add(type, 0);
            }
        }

        public void AddVehicle(int index, VehicleManager vehicleManager)
        {
            var vehicle = vehicleManager.m_vehicles.m_buffer[index];

            if ((vehicle.m_flags & (Vehicle.Flags.Created 
                                    | Vehicle.Flags.Deleted 
                                    | Vehicle.Flags.WaitingPath)) == Vehicle.Flags.Created)
            {
                var transport = GetTransportType(vehicle);
                this[transport]++;
            }
        }

        public void AddCitizen(int index, VehicleManager vehicleManager, CitizenManager citizenManager)
        {
            var citizenInstance = citizenManager.m_instances.m_buffer[index];
            var citizen = citizenManager.m_citizens.m_buffer[citizenInstance.m_citizen];

            if ((citizenInstance.m_flags & (CitizenInstance.Flags.Created 
                                            | CitizenInstance.Flags.Deleted 
                                            | CitizenInstance.Flags.WaitingPath)) == CitizenInstance.Flags.Created)
            {
                var transport = GetTransportType(citizen, vehicleManager);
                this[transport]++;
            }
        }

        private TransportType GetTransportType(Vehicle vehicle)
        {
            // based on PathVisualizer
            
            var info = vehicle.Info;
            var service = info.m_class.m_service;
            var subService = info.m_class.m_subService;
            var ai = info.m_vehicleAI;

            switch (service)
            {
                case ItemClass.Service.Residential:
                    return TransportType.Private;
                case ItemClass.Service.Industrial:
                    return TransportType.Truck;
                case ItemClass.Service.PublicTransport:
                    return subService == ItemClass.SubService.PublicTransportPost
                        ? TransportType.Truck
                        : TransportType.Public;
                case ItemClass.Service.Commercial:
                    return TransportType.Truck;
                case ItemClass.Service.Fishing:
                    return ai && ai is FishingBoatAI
                        ? TransportType.Truck
                        : TransportType.Public;
                default:
                    return TransportType.Service;
            }
        }

        private TransportType GetTransportType(Citizen citizen, VehicleManager vehicleManager)
        {
            var vehicleType = VehicleInfo.VehicleType.None;
            var vehicleId = citizen.m_vehicle;

            if (vehicleId != 0)
            {
                var vehicle = vehicleManager.m_vehicles.m_buffer[vehicleId];
                var info = vehicle.Info;

                if (info)
                {
                    vehicleType = info.m_vehicleType;
                }
            }

            switch (vehicleType)
            {
                case VehicleInfo.VehicleType.Bicycle:
                    return TransportType.Cyclist;
                default:
                    return TransportType.Pedestrian;
            }
        }
        
        // may use later
        public TransportType GetTransportType(VehicleAI ai)
        {
            // these are all the AIs that override their route color
            if (ai is PassengerCarAI passengerCarAI) return TransportType.Private;
            if (ai is CargoTruckAI cargoTruckAI) return TransportType.Truck;
            if (ai is BusAI busAI) return TransportType.Public;
            if (ai is CableCarAI cableCarAI) return TransportType.Public;
            if (ai is CargoPlaneAI cargoPlaneAI) return TransportType.Public;
            if (ai is CargoShipAI cargoShipAI) return TransportType.Public;
            if (ai is CargoTrainAI cargoTrainAI) return TransportType.Public;
            if (ai is FishingBoatAI fishingBoatAI) return TransportType.Public;
            if (ai is PassengerBlimpAI passengerBlimpAI) return TransportType.Public;
            if (ai is PassengerFerryAI passengerFerryAI) return TransportType.Public;
            if (ai is PassengerHelicopterAI passengerHelicopterAI) return TransportType.Public;
            if (ai is PassengerPlaneAI passengerPlaneAI) return TransportType.Public;
            if (ai is PassengerShipAI passengerShipAI) return TransportType.Public;
            if (ai is PassengerTrainAI passengerTrainAI) return TransportType.Public;
            if (ai is TaxiAI taxiAI) return TransportType.Public;
            if (ai is TramAI tramAI) return TransportType.Public;
            if (ai is TrolleybusAI trolleybusAI) return TransportType.Public;
            
            // service - default for all other VehicleAIs
            return TransportType.Service;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"Pedestrians: {this[TransportType.Pedestrian]}\n");
            builder.Append($"Cyclists: {this[TransportType.Cyclist]}\n");
            builder.Append($"Private vehicles: {this[TransportType.Private]}\n");
            builder.Append($"Public transport: {this[TransportType.Public]}\n");
            builder.Append($"Trucks: {this[TransportType.Truck]}\n");
            builder.Append($"City service: {this[TransportType.Service]}\n");

            return builder.ToString();
        }
    }
}