using ColossalFramework.Plugins;
using UnityEngine;

namespace TrafficVolume
{
    public class Log
    {
        private string _prefix;

        private const PluginManager.MessageType Info = PluginManager.MessageType.Message;

        public Log(string modID)
        {
            _prefix = $"[{modID}] ";
        }
        
        public void WriteInfo(string text)
        {
            Debug.Log(_prefix + text);
            
            // DebugOutputPanel.AddMessage(Info,_prefix + text);
        }
    }
}