using UnityEngine;
using HelloMod;
namespace HelloMod
{
    public class Main : IMod
    {
        public string Identifier { get; set; }
        GameObject _go;
        CoasterTypeLoader CTL;
        public void onEnabled()
        {
            _go = new GameObject("CoasterTypeLoaderGO");
            CTL =_go.AddComponent<CoasterTypeLoader>();
            CTL.Path = Path;
            CTL.CreateCustomCoasterType();
        }

        public void onDisabled()
        {
            CTL.UnregisterItems();
            UnityEngine.Object.DestroyImmediate(_go);
        }

        public string Name 
        {
            get { return "Corkscrew Coaster"; }
        }

        public string Path { get; set; }
        public string Description
        {
            get { return "Creates a Corkscrew Coaster"; }
        }
    }
}
