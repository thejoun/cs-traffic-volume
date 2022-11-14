using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.UI;
using TrafficVolume.Extensions;
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

        private static TrafficRadialChart _trafficChart;
        private static Volume _displayedVolume;

        private static readonly Dictionary<TransportType, CheckboxData> Checkboxes 
            = new Dictionary<TransportType, CheckboxData>();

        private static TrafficRoutesInfoViewPanel RoutesPanel => TrafficRoutesInfoViewPanel.instance;
        private static Color[] RouteColors => Singleton<InfoManager>.instance.m_properties.m_routeColors;

        private static void OnCheckboxCheckChanged(CheckboxData checkbox)
        {
            RefreshVolume();
        }

        public static void OnLevelLoaded()
        {
            // needs to be called on level loaded, after InfoManager instance is created
            // here we assume that checkboxes are already initialized
            PrepareColors();
        }

        public static void RegisterCheckboxes(List<CheckboxData> checkboxes)
        {
            Checkboxes.Clear();
            
            foreach (var checkboxData in checkboxes)
            {
                Checkboxes.Add(checkboxData.Transport, checkboxData);
                
                checkboxData.CheckChanged += OnCheckboxCheckChanged;
            }
            
            var trafficPanel = GetTrafficPanel();
            
            _trafficChart = CreateTrafficChart(trafficPanel);
        }

        public static void DisplayVolume(Volume volume)
        {
            _displayedVolume = volume;

            DisplayVolumeNumbers(volume);
            DisplayVolumeChart(volume);
        }

        private static void PrepareColors()
        {
            var checkboxes = Checkboxes.Values;
            
            foreach (var checkbox in checkboxes)
            {
                PrepareColor(checkbox);
            }

            if (_trafficChart)
            {
                _trafficChart.PrepareColors(checkboxes);
            }
        }

        private static void RefreshVolume()
        {
            if (_displayedVolume != null)
            {
                DisplayVolumeChart(_displayedVolume);
            }
        }

        private static void DisplayVolumeNumbers(Volume volume)
        {
            foreach (var kvp in volume)
            {
                var transport = kvp.Key;
                var count = kvp.Value;

                if (Checkboxes.TryGetValue(transport, out var checkbox))
                {
                    var label = checkbox.OriginalText;
                    var text = $"{count} {label}";
                    
                    checkbox.UiCheckbox.text = text;
                }
            }
        }

        private static void DisplayVolumeChart(Volume volume)
        {
            if (_trafficChart)
            {
                var chartVolume = new Volume(false);

                foreach (var transportVolume in volume)
                {
                    var type = transportVolume.Key;
                    var count = transportVolume.Value;
                    
                    if (Checkboxes.TryGetValue(type, out var checkbox))
                    {
                        var value = checkbox.IsChecked ? count : 0;
                        
                        chartVolume.Add(type, value);
                    }
                }

                _trafficChart.DisplayVolume(chartVolume);
            }
        }

        private static void PrepareColor(CheckboxData checkbox)
        {
            var transport = checkbox.Transport;
            var id = (int) transport;
            var primaryColor = RouteColors[id];

            if (transport == TransportType.Cyclist)
            {
                // cyclists - make them more blue and distinct from pedestrians
                primaryColor = primaryColor.ModifyHSV(0.05f, 0f, 0f);
            }

            var secondaryColor = primaryColor.ModifyHSV(-0.02f, -0.07f, -0.05f);
                
            checkbox.SetColors(primaryColor, secondaryColor);
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

            chart.Create(Checkboxes.Values);
            
            return chart;
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
    }
}