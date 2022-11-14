using HarmonyLib;
using ICities;

namespace TrafficVolume
{
    public class ModInfo : IUserMod
    {
        public const string HarmonyModID = "trafficvolume";

        public string Name => "Traffic Volume";
        public string Description => "shows info about traffic volume";

        public void OnEnabled()
        {
            var harmony = new Harmony(HarmonyModID);
            harmony.PatchAll();
        }
    }
}