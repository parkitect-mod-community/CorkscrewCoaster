using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HelloMod
{
    class CoasterTypeLoader : MonoBehaviour
    {
        public string Path;
        public List<UnityEngine.Object> loadedList = new List<UnityEngine.Object>();

        public void CreateCustomCoasterType()
        {
            TrackedRide TR = new TrackedRide();
            GameObject hider = new GameObject();

            char dsc = System.IO.Path.DirectorySeparatorChar;
            using (WWW www = new WWW("file://" + Path + dsc + "assetbundle" + dsc + "mod"))
            {
                AssetBundle bundle = www.assetBundle;
                List<Attraction> attractions = AssetManager.Instance.getAttractionObjects().ToList();
                foreach (TrackedRide attraction in attractions.OfType<TrackedRide>())
                {
                    if (attraction.getUnlocalizedName() == "Steel Coaster")
                    {
                        TR = Instantiate(attraction);
                        TR.dropsImportanceExcitement = 0.665f;
                        TR.inversionsImportanceExcitement = 0.673f;
                        TR.averageLatGImportanceExcitement = 0.121f;
                        TR.accelerationImportanceExcitement = 0.525f;
                        TR.meshGenerator = ScriptableObject.CreateInstance<CustomCoasterMeshGenerator>();
                        TR.meshGenerator.stationPlatformGO = attraction.meshGenerator.stationPlatformGO;
                        TR.meshGenerator.material = attraction.meshGenerator.material;
                        TR.meshGenerator.liftMaterial = attraction.meshGenerator.liftMaterial;
                        TR.meshGenerator.frictionWheelsGO = attraction.meshGenerator.frictionWheelsGO;
                        TR.meshGenerator.supportInstantiator = attraction.meshGenerator.supportInstantiator;
                        GameObject asset  = Instantiate(bundle.LoadAsset("Corkscrew Coaster@Crossbeam")) as GameObject;
                        TR.meshGenerator.crossBeamGO = SetUV(asset, 15, 14);
                        break;
                    }
                }


                /* foreach (TrackedRide attraction in attractions.OfType<TrackedRide>())
                {
                    if (attraction.getName() == "Log Flume")
                    {
                        TR.meshGenerator = Instantiate(attraction.meshGenerator);
                    }
                }*/

                int pillar = 89;
                int center = 100;
                int crossbeam = 255;
                int beams = 175;

                Color[] colors = new Color[] { new Color(pillar / 255f, pillar / 255f, pillar / 255f, 1), new Color(center / 255f, center / 255f, center / 255f, 1), new Color(crossbeam / 255f, crossbeam / 255f, crossbeam / 255f, 1), new Color(beams / 255f, beams / 255f, beams / 255f, 1) };
                TR.meshGenerator.customColors = colors;
                TR.meshGenerator.customColors = colors;
                TR.setDisplayName("Corkscrew Coaster");
                TR.price = 3600;
                TR.name = "Corkscrew_coaster_GO";
                AssetManager.Instance.registerObject(TR);

                //Get cars from assetbundle
                GameObject carGO = bundle.LoadAsset("Corkscrew Coaster@Car") as GameObject;
                Rigidbody carRigid = carGO.AddComponent<Rigidbody>();
                carRigid.isKinematic = true;
                carGO.AddComponent<BoxCollider>();
                GameObject frontCarGO = bundle.LoadAsset("Corkscrew Coaster@FrontCar") as GameObject;
                Rigidbody frontCarRigid = frontCarGO.AddComponent<Rigidbody>();
                frontCarRigid.isKinematic = true;
                frontCarGO.AddComponent<BoxCollider>();

                //Add car type 
                CustomCar car = carGO.AddComponent<CustomCar>();
                CustomCar frontCar = frontCarGO.AddComponent<CustomCar>();

                car.Decorate(carGO, false);
                frontCar.Decorate(frontCarGO, true);

                //Create Instantiator
                List<CoasterCarInstantiator> TypeList = new List<CoasterCarInstantiator>();
                CoasterCarInstantiator CarInstantiator = ScriptableObject.CreateInstance<CoasterCarInstantiator>();

                CarInstantiator.name = "Corkscrew Coaster@CoasterCarInstantiator";
                CarInstantiator.defaultTrainLength = 5;
                CarInstantiator.maxTrainLength = 7;
                CarInstantiator.minTrainLength = 2;
                CarInstantiator.carGO = carGO;
                CarInstantiator.frontCarGO = frontCarGO;

                //Register
                AssetManager.Instance.registerObject(car);
                AssetManager.Instance.registerObject(frontCar);

                //Offset
                float CarOffset = .02f;
                car.offsetBack = CarOffset;
                frontCar.offsetBack = CarOffset;

                //Restraints
                RestraintRotationController controller = carGO.AddComponent<RestraintRotationController>();
                RestraintRotationController controllerFront = frontCarGO.AddComponent<RestraintRotationController>();
                controller.closedAngles = new Vector3(0,0,120);
                controllerFront.closedAngles = new Vector3(0, 0, 120);
                
                //Custom Colors
                Color[] CarColors = new Color[] { new Color(168f / 255, 14f / 255, 14f / 255), new Color(234f / 255, 227f / 255, 227f / 255), new Color(73f / 255, 73f / 255, 73f / 255) };

                MakeRecolorble(frontCarGO, "CustomColorsDiffuse", CarColors);
                MakeRecolorble(carGO, "CustomColorsDiffuse", CarColors);

                CarInstantiator.displayName = "Corkscrew Car";
                AssetManager.Instance.registerObject(CarInstantiator);
                TypeList.Add(CarInstantiator);

                TR.carTypes = TypeList.ToArray();

                bundle.Unload(false);

                carGO.transform.parent = hider.transform;
                frontCarGO.transform.parent = hider.transform;
                hider.SetActive(false);

                loadedList.Add(TR);
                loadedList.Add(car);
                loadedList.Add(frontCar);
                loadedList.Add(CarInstantiator);

                Debug.Log(TR.getName() + " Loaded");
            }
        }

        public void UnregisterItems()
        {
            foreach(UnityEngine.Object obj in loadedList)
            {
                AssetManager.Instance.unregisterObject(obj);
            }
        }

        void MakeRecolorble(GameObject GO, string shader, Color[] colors)
        {
            CustomColors cc = GO.AddComponent<CustomColors>();
            cc.setColors(colors);

            foreach (Material material in AssetManager.Instance.objectMaterials)
            {
                if (material.name == shader)
                {
                    SetMaterial(GO, material);
                    break;
                }
            }

        }
        GameObject SetUV(GameObject GO, int gridX, int gridY)
        {
            Mesh mesh = GO.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            Vector2[] uvs = new Vector2[vertices.Length];

            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(0.0625f * ((float)gridX + 0.5f), 1f - 0.0625f * ((float)gridY + 0.5f));
            }
            mesh.uv = uvs;
            return GO;
        }
        public void SetMaterial(GameObject go, Material material)
        {
            // Go through all child objects and recolor		
            Renderer[] renderCollection;
            renderCollection = go.GetComponentsInChildren<Renderer>();

            foreach (Renderer render in renderCollection)
            {
                render.sharedMaterial = material;
            }
        }
    }

}
