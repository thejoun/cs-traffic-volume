using ICities;
using TrafficVolume.Managers;

namespace TrafficVolume.CitiesExtensions
{
	public class ThreadingExtension : ThreadingExtensionBase
    {
	    public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
		{
			// allows stepping into this code with a debugger
			base.OnUpdate(realTimeDelta, simulationTimeDelta);
			
			Manager.OnSimulationUpdate(realTimeDelta, simulationTimeDelta);
		}
    }
}
