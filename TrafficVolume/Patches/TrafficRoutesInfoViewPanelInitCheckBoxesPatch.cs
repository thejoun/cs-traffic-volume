using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using HarmonyLib;

namespace TrafficVolume.Patches
{
    [HarmonyPatch(typeof(TrafficRoutesInfoViewPanel), "InitCheckBoxes")]
    public class TrafficRoutesInfoViewPanelInitCheckBoxesPatch
    {
        public static void Postfix(
            UICheckBox ___m_PedestrianCheckBox,
            UICheckBox ___m_CyclistCheckBox,
            UICheckBox ___m_PrivateVehicleCheckBox, 
            UICheckBox ___m_PublicTransportCheckBox,
            UICheckBox ___m_TrucksCheckBox,
            UICheckBox ___m_CityServiceCheckBox)
        {
            Manager.CheckBoxDict = new Dictionary<Transport, UICheckBox>()
            {
                {Transport.Pedestrian, ___m_PedestrianCheckBox},
                {Transport.Cyclist, ___m_CyclistCheckBox},
                {Transport.Private, ___m_PrivateVehicleCheckBox},
                {Transport.Public, ___m_PublicTransportCheckBox},
                {Transport.Truck, ___m_TrucksCheckBox},
                {Transport.Service, ___m_CityServiceCheckBox}
            };

            Manager.TransportNameDict = new Dictionary<Transport, string>()
            {
                {Transport.Pedestrian, ___m_PedestrianCheckBox.text},
                {Transport.Cyclist, ___m_CyclistCheckBox.text},
                {Transport.Private, ___m_PrivateVehicleCheckBox.text},
                {Transport.Public, ___m_PublicTransportCheckBox.text},
                {Transport.Truck, ___m_TrucksCheckBox.text},
                {Transport.Service, ___m_CityServiceCheckBox.text}
            };
        }
    }
}