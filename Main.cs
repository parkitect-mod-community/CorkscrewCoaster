using System.Collections.Generic;
using System.Linq;
using TrackedRiderUtility;
using UnityEngine;

namespace CorkscrewCoaster
{
    public class Main : IMod
    {
        private TrackRiderBinder binder;
        private AssetBundle assetBundle;
        
        public GameObject FrontCartGo;
        public GameObject CartGo;
        public GameObject SideCrossBeamsGo;

        
        GameObject _go;

        public void onEnabled()
        {
            var dsc = System.IO.Path.DirectorySeparatorChar;
            assetBundle = AssetBundle.LoadFromFile( Path + dsc + "assetpack");

            FrontCartGo =  assetBundle.LoadAsset<GameObject> ("01be2cec49bbb476381a537d75ad047e");
            CartGo =  assetBundle.LoadAsset<GameObject> ("7c1045f838c59460db2bfebd3df04a47");
            SideCrossBeamsGo =  assetBundle.LoadAsset<GameObject> ("c184c4f392587465f9bf2c86e6615e78");
            
            binder = new TrackRiderBinder("kvwQwhKWWG");
            TrackedRide trackedRide =
                binder.RegisterTrackedRide<TrackedRide>("Steel Coaster", "CorkscrewCoaster", "Corkscrew Coaster");
            TwisterCoasterMeshGenerator trackGenerator =
                binder.RegisterMeshGenerator<TwisterCoasterMeshGenerator>(trackedRide);
            TrackRideHelper.PassMeshGeneratorProperties(TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator,
                trackedRide.meshGenerator);

            trackGenerator.crossBeamGO = GameObjectHelper.SetUV(Object.Instantiate(SideCrossBeamsGo), 15, 14);


            trackedRide.price = 1200;
            trackedRide.carTypes = new CoasterCarInstantiator[] { };
            trackedRide.meshGenerator.customColors = new[]
            {
                new Color(63f / 255f, 46f / 255f, 37f / 255f, 1), new Color(43f / 255f, 35f / 255f, 35f / 255f, 1),
                new Color(90f / 255f, 90f / 255f, 90f / 255f, 1),new Color(90f / 255f, 90f / 255f, 90f / 255f, 1)
            };
            trackedRide.dropsImportanceExcitement = 0.665f;
            trackedRide.inversionsImportanceExcitement = 0.673f;
            trackedRide.averageLatGImportanceExcitement = 0.121f;
            trackedRide.accelerationImportanceExcitement = 0.525f;

            CoasterCarInstantiator coasterCarInstantiator =
                binder.RegisterCoasterCarInstaniator<CoasterCarInstantiator>(trackedRide, "CorkscrewCoasterInsantiator",
                    "Corkscrew Car", 6, 16, 3);

            BaseCar frontCar = binder.RegisterCar<BaseCar>(Object.Instantiate(FrontCartGo), "CorkScrewCoaster_Front_Car",
                .35f, 0f, true, new[]
                {
                    new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255),
                    new Color(73f / 255, 73f / 255, 73f / 255)
                }
            );
            coasterCarInstantiator.frontVehicleGO = frontCar;
            coasterCarInstantiator.frontVehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);
           

            List<Transform> transforms = new List<Transform>();
            Utility.recursiveFindTransformsStartingWith("wheel", frontCar.transform, transforms);
            foreach (var transform in transforms)
            {
                transform.gameObject.AddComponent<FrictionWheelAnimator>();
            }

            BaseCar backCar = binder.RegisterCar<BaseCar>(Object.Instantiate(CartGo), "CorkScrewCoaster_Back_Car", .35f,
                -.3f, false, new[]
                {
                    new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255),
                    new Color(73f / 255, 73f / 255, 73f / 255)
                }
            );
            coasterCarInstantiator.vehicleGO = backCar;
            coasterCarInstantiator.vehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);

            Utility.recursiveFindTransformsStartingWith("wheel", backCar.transform, transforms);
            foreach (var transform in transforms)
            {
                transform.gameObject.AddComponent<FrictionWheelAnimator>();
            }

            binder.Apply();
            assetBundle.Unload(false);
        }

        public void onDisabled()
        {
            binder.Unload();
        }
        
        public string Name => "Corkscrew Coaster";

        public string Description => "Creates a Corkscrew Coaster";

        string IMod.Identifier => "CorkscrewCoaster";
	
	
        public string Path
        {
            get
            {
                return ModManager.Instance.getModEntries().First(x => x.mod == this).path;
            }
        }
    }
}
