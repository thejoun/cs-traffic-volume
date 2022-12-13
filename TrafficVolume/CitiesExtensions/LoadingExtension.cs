using ICities;
using TrafficVolume.Managers;

namespace TrafficVolume.CitiesExtensions
{
	public class LoadingExtension : LoadingExtensionBase
    {
	    public override void OnLevelLoaded(LoadMode mode)
		{
			// allows stepping into this code with a debugger
			base.OnLevelLoaded(mode);
		
			Manager.OnLevelLoaded();
			UIManager.OnLevelLoaded();
		}
    }
}
