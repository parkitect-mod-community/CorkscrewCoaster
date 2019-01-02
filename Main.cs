/**
* Copyright 2019 Michael Pollind
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Collections.Generic;
using System.Linq;
using TrackedRiderUtility;
using UnityEngine;

namespace CorkscrewCoaster
{
    public class Main : IMod
    {
        public static AssetBundleManager AssetBundleManager;
        private TrackRiderBinder binder;


        public string Path
        {
            get { return ModManager.Instance.getModEntries().First(x => x.mod == this).path; }
        }

        public void onEnabled()
        {
            if (AssetBundleManager == null) AssetBundleManager = new AssetBundleManager(this);

            binder = new TrackRiderBinder("kvwQwhKWWG");
            var trackedRide =
                binder.RegisterTrackedRide<TrackedRide>("Steel Coaster", "CorkscrewCoaster", "Corkscrew Coaster");
            var trackGenerator =
                binder.RegisterMeshGenerator<TwisterCoasterMeshGenerator>(trackedRide);
            TrackRideHelper.PassMeshGeneratorProperties(TrackRideHelper.GetTrackedRide("Steel Coaster").meshGenerator,
                trackedRide.meshGenerator);

            trackGenerator.crossBeamGO =
                GameObjectHelper.SetUV(Object.Instantiate(AssetBundleManager.SideCrossBeamsGo), 15, 14);


            trackedRide.price = 1200;
            trackedRide.carTypes = new CoasterCarInstantiator[] { };
            trackedRide.meshGenerator.customColors = new[]
            {
                new Color(63f / 255f, 46f / 255f, 37f / 255f, 1), new Color(43f / 255f, 35f / 255f, 35f / 255f, 1),
                new Color(90f / 255f, 90f / 255f, 90f / 255f, 1), new Color(90f / 255f, 90f / 255f, 90f / 255f, 1)
            };
            trackedRide.dropsImportanceExcitement = 0.665f;
            trackedRide.inversionsImportanceExcitement = 0.673f;
            trackedRide.averageLatGImportanceExcitement = 0.121f;
            trackedRide.accelerationImportanceExcitement = 0.525f;

            var coasterCarInstantiator =
                binder.RegisterCoasterCarInstaniator<CoasterCarInstantiator>(trackedRide, "CorkscrewCoasterInsantiator",
                    "Corkscrew Car", 6, 16, 3);

            var frontCar = binder.RegisterCar<BaseCar>(Object.Instantiate(AssetBundleManager.FrontCartGo),
                "CorkScrewCoaster_Front_Car",
                .35f, 0f, true, new[]
                {
                    new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255),
                    new Color(73f / 255, 73f / 255, 73f / 255)
                }
            );
            coasterCarInstantiator.frontVehicleGO = frontCar;
            coasterCarInstantiator.frontVehicleGO.gameObject.AddComponent<RestraintRotationController>().closedAngles =
                new Vector3(110, 0, 0);


            var transforms = new List<Transform>();
            Utility.recursiveFindTransformsStartingWith("wheel", frontCar.transform, transforms);
            foreach (var transform in transforms) transform.gameObject.AddComponent<FrictionWheelAnimator>();

            var backCar = binder.RegisterCar<BaseCar>(Object.Instantiate(AssetBundleManager.CartGo),
                "CorkScrewCoaster_Back_Car", .35f,
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
            foreach (var transform in transforms) transform.gameObject.AddComponent<FrictionWheelAnimator>();

            binder.Apply();
        }

        public void onDisabled()
        {
            binder.Unload();
        }

        public string Name => "Corkscrew Coaster";

        public string Description => "Creates a Corkscrew Coaster";

        string IMod.Identifier => "CorkscrewCoaster";
    }
}