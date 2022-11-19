using ICities;
using TrafficVolume.Managers;

namespace TrafficVolume.CitiesExtensions
{
	public class LoadingExtension : LoadingExtensionBase
    {
	    public override void OnLevelLoaded(LoadMode mode)
		{
			Manager.OnLevelLoaded();
			UIManager.OnLevelLoaded();
		}
    }
}
