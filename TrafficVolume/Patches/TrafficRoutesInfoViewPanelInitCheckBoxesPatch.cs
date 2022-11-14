using System.Collections.Generic;
using ColossalFramework.UI;
using HarmonyLib;
using TrafficVolume.UI;

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
            var checkboxes = new List<CheckboxData>
            {
                new CheckboxData(___m_PedestrianCheckBox, TransportType.Pedestrian),
                new CheckboxData(___m_CyclistCheckBox, TransportType.Cyclist),
                new CheckboxData(___m_PrivateVehicleCheckBox, TransportType.Private),
                new CheckboxData(___m_PublicTransportCheckBox, TransportType.Public),
                new CheckboxData(___m_TrucksCheckBox, TransportType.Truck),
                new CheckboxData(___m_CityServiceCheckBox, TransportType.Service)
            };
            
            UIManager.RegisterCheckboxes(checkboxes);
        }
    }
}