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

using UnityEngine;

namespace CorkscrewCoaster
{
    public class AssetBundleManager
    {
        private readonly Main _main;
        private readonly AssetBundle assetBundle;
        public readonly GameObject CartGo;
        public readonly GameObject FrontCartGo;
        public readonly GameObject SideCrossBeamsGo;

        public AssetBundleManager(Main main)
        {
            _main = main;
            var dsc = System.IO.Path.DirectorySeparatorChar;
            assetBundle = AssetBundle.LoadFromFile(_main.Path + dsc + "assetbundle" + dsc + "assetpack");

            FrontCartGo = assetBundle.LoadAsset<GameObject>("01be2cec49bbb476381a537d75ad047e");
            CartGo = assetBundle.LoadAsset<GameObject>("7c1045f838c59460db2bfebd3df04a47");
            SideCrossBeamsGo = assetBundle.LoadAsset<GameObject>("c184c4f392587465f9bf2c86e6615e78");
            assetBundle.Unload(false);
        }

    }
}