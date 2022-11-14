using ICities;

namespace TrafficVolume.CitiesExtensions
{
	public class ThreadingExtension : ThreadingExtensionBase
    {
	    public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
		{
			Manager.OnSimulationUpdate(realTimeDelta, simulationTimeDelta);
		}
    }
}
