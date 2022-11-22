using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using UnityEngine;

namespace TrafficVolume
{
    public static class Settings
    {
        private enum IntervalOption
        {
            S1, S2, S5, S10, S01, S025, S05
        } 
        
        private const string FileName = "TrafficVolume";

        private const bool DefaultRefreshEnabled = false;
        private const int DefaultIntervalOption = 1;

        
        private static Dictionary<IntervalOption, string> _intervalOptionText = new Dictionary<IntervalOption, string>()
        {
            {IntervalOption.S01, "0.1s"},
            {IntervalOption.S025, "0.25s"},
            {IntervalOption.S05, "0.5s"},
            {IntervalOption.S1, "1s"},
            {IntervalOption.S2, "2s (default)"},
            {IntervalOption.S5, "5s"},
            {IntervalOption.S10, "10s"}
        };
        
        private static Dictionary<IntervalOption, float> _intervalOptionTime = new Dictionary<IntervalOption, float>()
        {
            {IntervalOption.S01, 0.1f},
            {IntervalOption.S025, 0.25f},
            {IntervalOption.S05, 0.5f},
            {IntervalOption.S1, 1f},
            {IntervalOption.S2, 2f},
            {IntervalOption.S5, 5f},
            {IntervalOption.S10, 10f}
        };

        private static SavedBool autoRefreshEnabled = 
            new SavedBool(nameof(autoRefreshEnabled), FileName, DefaultRefreshEnabled, true);

        private static SavedInt autoRefreshInterval =
            new SavedInt(nameof(autoRefreshInterval), FileName, DefaultIntervalOption, true);

        private static SavedInputKey globalTrafficKeybind =
            new SavedInputKey(nameof(globalTrafficKeybind), FileName, DefaultGlobalTrafficInput, true);

        private static InputKey DefaultGlobalTrafficInput => SavedInputKey.Encode(KeyCode.G, true, false, false);

        public static string[] AutoRefreshIntervalOptions => _intervalOptionText.Values.ToArray();
        public static bool IsAutoRefreshEnabled => autoRefreshEnabled.value;
        public static SavedInputKey GlobalTrafficKeybind => globalTrafficKeybind;

        public static void SetAutoRefreshEnabled(bool value)
        {
            autoRefreshEnabled.value = value;
        }

        public static void SetAutoRefreshIntervalOptionNumber(int optionNumber)
        {
            var option = _intervalOptionText.ElementAtOrDefault(optionNumber);
            
            autoRefreshInterval.value = (int)option.Key;
        }

        public static int GetAutoRefreshIntervalOptionNumber()
        {
            var option = GetIntervalOption();
            
            for (int i = 0; i < _intervalOptionText.Count; i++)
            {
                var element = _intervalOptionText.ElementAt(i);

                if (element.Key == option)
                {
                    return i;
                }
            }

            return 0;
        }
        
        public static float GetAutoRefreshInterval()
        {
            var option = GetIntervalOption();

            if (_intervalOptionTime.TryGetValue(option, out var time))
            {
                return time;
            }

            return 1f;
        }

        private static IntervalOption GetIntervalOption()
        {
            var value = autoRefreshInterval.value;

            if (!Enum.IsDefined(typeof(IntervalOption), value))
            {
                value = DefaultIntervalOption;
            }

            var option = (IntervalOption) value;
            
            return option;
        }
    }
}