using System.Collections.Generic;
using HarmonyLib;

namespace TrafficVolume.Patches
{
    [HarmonyPatch(typeof(PathVisualizer), "AddPathsImpl")]
    public class PathVisualizerAddPathsImplPatch
    {
        public static void Prefix(int min, int max, HashSet<InstanceID> ___m_targets)
        {
            if (min == 0 && max == 256)
            {
                LocalTraffic.CountVolume(___m_targets);
                
                // Manager.Log.WriteInfo("prefix");
                // LocalTraffic.BeginCounting();
            }
        }

        // public static void Postfix(int min, int max)
        // {
        //     if (min == 0 && max == 256)
        //     {
        //         Manager.Log.WriteInfo("postfix");
        //         LocalTraffic.FinishCounting();
        //     }
        // }
    }
}