using ICities;

namespace TrafficVolume.CitiesExtensions
{
	public class ThreadingExtension : ThreadingExtensionBase
    {
	    //Thread: Main
		public override void OnCreated(IThreading threading)
		{
			
		}
		
		//Thread: Main
		public override void OnReleased()
		{
			
		}

		public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
		{
			base.OnUpdate(realTimeDelta, simulationTimeDelta);
			
			Manager.OnSimulationUpdate(realTimeDelta, simulationTimeDelta);
		}
    }
}
