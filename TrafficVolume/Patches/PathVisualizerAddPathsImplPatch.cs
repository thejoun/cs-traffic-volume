using System.Collections.Generic;
using HarmonyLib;
using TrafficVolume.Managers;
using TrafficVolume.Traffic;

namespace TrafficVolume.Patches
{
    [HarmonyPatch(typeof(PathVisualizer), "AddPathsImpl")]
    public class PathVisualizerAddPathsImplPatch
    {
        // called when a road segment is selected
        public static void Prefix(int min, int max, HashSet<InstanceID> ___m_targets)
        {
            if (min == 0 && max == 256)
            {
                var volume = LocalTraffic.CountLocalVolume(___m_targets);
                
                UIManager.DisplayVolume(volume);
                
                Manager.ResetRefreshTimer();
            }
        }
    }
}