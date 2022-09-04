using ICities;

namespace TrafficVolume.CitiesExtensions
{
	public class LoadingExtension : LoadingExtensionBase
    {
	    // Thread: Main
		public override void OnCreated(ILoading loading)
		{
			
		}
		
		// Thread: Main
		public override void OnReleased()
		{
			
		}

		public override void OnLevelLoaded(LoadMode mode)
		{
			base.OnLevelLoaded(mode);
			
			Manager.OnLevelLoaded(mode);
		}
    }
}
