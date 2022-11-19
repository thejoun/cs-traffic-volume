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
        
        public void Write(string text)
        {
            WriteConsole(text);
            WriteLog(text);
        }
        
        public void WriteConsole(string text)
        {
            DebugOutputPanel.AddMessage(Info,_prefix + text);
        }
        
        public void WriteLog(string text)
        {
            Debug.Log(_prefix + text);
        }
    }
}