using ICities;

namespace TrafficVolume
{
    public class ModInfo : IUserMod
    {
        public string Name => "Traffic Volume";
        public string Description => $"[{Manager.VersionId}] Shows info about traffic volume.";

        public void OnEnabled()
        {
            Manager.OnModEnabled();
        }
    }
}