using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HelloMod
{
    class CustomCar : Car
    {
        public void Decorate(GameObject GO, bool isFront)
        {
            backAxis = GO.transform.FindChild("backWheels").gameObject.transform;
            if(isFront)
                frontAxis = GO.transform.FindChild("frontWheels").gameObject.transform;
        }
    }
}
