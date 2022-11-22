using System;
using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using TrafficVolume.Managers;
using UnityEngine;

namespace TrafficVolume
{
    public class ModInfo : IUserMod
    {
        public const string LogFlag = "trafficvolume";

        private const string HarmonyModID = "trafficvolume";
        private const string SettingsFileName = "TrafficVolume";

        public string Name => "Traffic Volume";
        public string Description => "shows info about traffic volume";

        public ModInfo()
        {
            try
            {
                if (GameSettings.FindSettingsFileByName(SettingsFileName) != null)
                {
                    return;
                }
                
                GameSettings.AddSettingsFile(new SettingsFile()
                {
                    fileName = SettingsFileName
                });
            }
            catch (Exception ex)
            {
                Manager.Log.WriteLog("Could not load/create the setting file. " + ex);
            }
        }

        public void OnEnabled()
        {
            var harmony = new Harmony(HarmonyModID);
            harmony.PatchAll();
        }

        public void OnSettingsUI(UIHelperBase ui)
        {
            try
            {
                var autoRefreshGroup = ui.AddGroup("Auto refresh");

                var enableAutoRefreshCheckbox = (UIComponent) autoRefreshGroup.AddCheckbox(
                    "Enable auto refresh",
                    Settings.IsAutoRefreshEnabled,
                    Settings.SetAutoRefreshEnabled
                    );
                    
                enableAutoRefreshCheckbox.tooltip = "Check to make traffic volume values refresh automatically";
                
                var autoRefreshInterval = (UIComponent) autoRefreshGroup.AddDropdown(
                    "Auto refresh interval",
                    Settings.AutoRefreshIntervalOptions,
                    Settings.GetAutoRefreshIntervalOptionNumber(),
                    Settings.SetAutoRefreshIntervalOptionNumber);

                autoRefreshInterval.tooltip = "How much time passes before the values are refreshed";
                
                var globalTrafficGroup = ui.AddGroup("Global traffic");
                var globalTrafficGroupGo = ((Component) ((UIHelper) globalTrafficGroup).self).gameObject;

                var globalTrafficKeymapping = globalTrafficGroupGo.AddComponent<Keymapping>();
                globalTrafficKeymapping.AddKeymapping("Open global traffic volume", Settings.GlobalTrafficKeybind);
            }
            catch (Exception ex)
            {
                Manager.Log.WriteLog("OnSettingsUI failed. " + ex);
            }
        }
    }
}