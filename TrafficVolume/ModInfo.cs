using ICities;

namespace TrafficVolume
{
    public class ModInfo : IUserMod
    {
        public string Name
        {
            get { return "Traffic Volume"; }
        }

        public string Description
        {
            get { return "Shows info about traffic volume."; }
        }

        public void OnEnabled()
        {
            Manager.OnModEnabled();
        }
    }
}