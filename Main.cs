using System.Linq;
using TrackedRiderUtility;
using UnityEngine;

namespace CorkscrewCoaster
{
    public class Main : IMod
    {
        public static AssetBundleManager AssetBundleManager;
  
        private TrackRiderBinder binder;

        
        public string Identifier { get; set; }
        GameObject _go;
        
        public void onEnabled()
        {
            if (AssetBundleManager == null)
            {
                AssetBundleManager = new AssetBundleManager(this);
            }
            binder = new TrackRiderBinder("da14b20fb34ccbeda5add956f8dde549");
            TrackedRide trackedRide = binder.RegisterTrackedRide<TrackedRide> ("Steel Coaster","CorkscrewCoaster", "Corkscrew Coaster");
            CustomCoasterMeshGenerator trackGenerator = binder.RegisterMeshGenerator<CustomCoasterMeshGenerator> (trackedRide);
            TrackRideHelper.PassMeshGeneratorProperties (TrackRideHelper.GetTrackedRide ("Steel Coaster").meshGenerator,trackedRide.meshGenerator);

            
            binder.Apply();
        }

        public void onDisabled()
        {
            binder.Unload();
        }
        
        public string Name => "Corkscrew Coaster";

        public string Description => ".";

        string IMod.Identifier => "VirginiaReel";
	
	
        public string Path
        {
            get
            {
                return ModManager.Instance.getModEntries().First(x => x.mod == this).path;
            }
        }
    }
}
