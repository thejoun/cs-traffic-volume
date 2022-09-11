using System;
using System.Collections.Generic;
using System.Text;

namespace TrafficVolume
{
    public class Volume : Dictionary<Transport, uint>
    {
        public Volume()
        {
            Clear();
        }

        public new void Clear()
        {
            base.Clear();
            
            var types = (Transport[]) Enum.GetValues(typeof(Transport));

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

        public Transport GetTransportType(Vehicle vehicle)
        {
            // based on PathVisualizer
            
            var info = vehicle.Info;
            var service = info.m_class.m_service;
            var subService = info.m_class.m_subService;
            var ai = info.m_vehicleAI;

            switch (service)
            {
                case ItemClass.Service.Residential:
                    return Transport.Private;
                case ItemClass.Service.Industrial:
                    return Transport.Truck;
                case ItemClass.Service.PublicTransport:
                    return subService == ItemClass.SubService.PublicTransportPost
                        ? Transport.Truck
                        : Transport.Public;
                case ItemClass.Service.Commercial:
                    return Transport.Truck;
                case ItemClass.Service.Fishing:
                    return ai && ai is FishingBoatAI
                        ? Transport.Truck
                        : Transport.Public;
                default:
                    return Transport.Service;
            }
        }

        public Transport GetTransportType(Citizen citizen, VehicleManager vehicleManager)
        {
            var vehicleType = VehicleInfo.VehicleType.None;
            var vehicleId = citizen.m_vehicle;

            if (vehicleId != 0)
            {
                var vehicle = vehicleManager.m_vehicles.m_buffer[vehicleId];
                var info = vehicle.Info;

                if (info != null)
                {
                    vehicleType = info.m_vehicleType;
                }
            }

            switch (vehicleType)
            {
                case VehicleInfo.VehicleType.Bicycle:
                    return Transport.Cyclist;
                default:
                    return Transport.Pedestrian;
            }
        }

        public Transport GetTransportType(VehicleAI ai)
        {
            // these are all the AIs that override their route color
            if (ai is PassengerCarAI passengerCarAI) return Transport.Private;
            if (ai is CargoTruckAI cargoTruckAI) return Transport.Truck;
            if (ai is BusAI busAI) return Transport.Public;
            if (ai is CableCarAI cableCarAI) return Transport.Public;
            if (ai is CargoPlaneAI cargoPlaneAI) return Transport.Public;
            if (ai is CargoShipAI cargoShipAI) return Transport.Public;
            if (ai is CargoTrainAI cargoTrainAI) return Transport.Public;
            if (ai is FishingBoatAI fishingBoatAI) return Transport.Public;
            if (ai is PassengerBlimpAI passengerBlimpAI) return Transport.Public;
            if (ai is PassengerFerryAI passengerFerryAI) return Transport.Public;
            if (ai is PassengerHelicopterAI passengerHelicopterAI) return Transport.Public;
            if (ai is PassengerPlaneAI passengerPlaneAI) return Transport.Public;
            if (ai is PassengerShipAI passengerShipAI) return Transport.Public;
            if (ai is PassengerTrainAI passengerTrainAI) return Transport.Public;
            if (ai is TaxiAI taxiAI) return Transport.Public;
            if (ai is TramAI tramAI) return Transport.Public;
            if (ai is TrolleybusAI trolleybusAI) return Transport.Public;
            
            // service - default for all other VehicleAIs
            return Transport.Service;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"Pedestrians: {this[Transport.Pedestrian]}\n");
            builder.Append($"Cyclists: {this[Transport.Cyclist]}\n");
            builder.Append($"Private vehicles: {this[Transport.Private]}\n");
            builder.Append($"Public transport: {this[Transport.Public]}\n");
            builder.Append($"Trucks: {this[Transport.Truck]}\n");
            builder.Append($"City service: {this[Transport.Service]}\n");

            return builder.ToString();
        }
    }
}