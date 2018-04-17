using UnityEngine;
using CorkscrewCoaster;

public class AssetBundleManager
{
    private Main Main {get;set;}
    private readonly Main _main;
    private AssetBundle assetBundle;

    public readonly GameObject FrontCartGo;
    public readonly GameObject CartGo;
    public readonly GameObject SideCrossBeamsGo;

    public AssetBundleManager (Main main)
    {
        _main = main;
        var dsc = System.IO.Path.DirectorySeparatorChar;
        assetBundle = AssetBundle.LoadFromFile( _main.Path + dsc + "assetbundle" + dsc + "assetpack");

        FrontCartGo =  assetBundle.LoadAsset<GameObject> ("01be2cec49bbb476381a537d75ad047e");
        CartGo =  assetBundle.LoadAsset<GameObject> ("7c1045f838c59460db2bfebd3df04a47");
        SideCrossBeamsGo =  assetBundle.LoadAsset<GameObject> ("c184c4f392587465f9bf2c86e6615e78");
    }

}