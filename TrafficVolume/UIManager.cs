using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.UI;
using Template.Extensions;
using TrafficVolume.UI;
using UnityEngine;

namespace TrafficVolume
{
    public class UIManager
    {
        private static readonly Vector2 TrafficChartPosition = new Vector2(275, -41);

        public static readonly Color BackgroundTopColor = new Color(90f / 255f, 96f / 255f, 105f / 255f);
        public static readonly Color BackgroundBottomColor = new Color(76f / 255f, 81f / 255f, 89f / 255f);
        public static readonly Color BlueTextColor = new Color(161f / 255f, 191f / 255f, 220f / 255f);
        
        public const int TrafficChartSize = 90;

        public static TrafficRadialChart TrafficChart { get; private set; }
        
        public static Dictionary<Transport, UICheckBox> TransportCheckBoxes { get; set; } 
            = new Dictionary<Transport, UICheckBox>();
        
        public static Dictionary<Transport, string> TransportLabels { get; set; } 
            = new Dictionary<Transport, string>();

        public static Dictionary<Transport, Color> TransportPrimaryColors { get; private set; }
            = new Dictionary<Transport, Color>();

        public static Dictionary<Transport, Color> TransportSecondaryColors { get; private set; }
            = new Dictionary<Transport, Color>();
        
        private static TrafficRoutesInfoViewPanel RoutesPanel => TrafficRoutesInfoViewPanel.instance;

        public static void PrepareColors()
        {
            var colors = Singleton<InfoManager>.instance.m_properties.m_routeColors;
            
            var types = (Transport[]) Enum.GetValues(typeof(Transport));

            foreach (var transport in types)
            {
                var id = (int) transport;
                var primaryColor = colors[id];

                if (transport == Transport.Cyclist)
                {
                    // cyclists - make them more blue and distinct from pedestrians
                    primaryColor = primaryColor.ModifyHSV(0.05f, 0f, 0f);
                }

                var secondaryColor = primaryColor.ModifyHSV(-0.02f, -0.07f, -0.05f);
                
                TransportPrimaryColors.Add(transport, primaryColor);
                TransportSecondaryColors.Add(transport, secondaryColor);
            }
        }

        public static void CreateTrafficChart()
        {
            var panel = GetTrafficPanel();
            TrafficChart = CreateTrafficChart(panel);
        }

        public static void DisplayTrafficCount(Volume volume)
        {
            var checkBoxDict = TransportCheckBoxes;
            var transportNameDict = TransportLabels;

            foreach (var kvp in volume.Dict)
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

        private static UIPanel GetTrafficPanel()
        {
            var routesPanel = RoutesPanel;
        
            if (routesPanel == null)
            {
                Manager.Log.Write("Routes panel is null");
                return null;
            }

            var transportComponent = routesPanel.Find("ShowTransportTypes");
            
            if (transportComponent == null)
            {
                Manager.Log.Write("Transport component is null");
                return null;
            }

            var transportPanel = transportComponent.gameObject.GetComponent<UIPanel>();
            
            if (transportPanel == null)
            {
                Manager.Log.Write("Transport panel is null");
                return null;
            }

            return transportPanel;
        }

        private static TrafficRadialChart CreateTrafficChart(UIPanel transportPanel)
        {
            transportPanel.autoLayout = false;

            var chart = transportPanel.AddUIComponent<TrafficRadialChart>();
            chart.size = new Vector2(TrafficChartSize, TrafficChartSize);
            chart.spriteName = "PieChartWhiteBg";

            chart.transform.localPosition = Vector2.zero;
            chart.arbitraryPivotOffset = TrafficChartPosition;

            chart.gameObject.name = "TrafficVolumePieChart";

            chart.Create();
            
            return chart;
        }
    }
}