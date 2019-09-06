

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OrderSystem.Common.UnityExpand
{
    public class UtilityImage : Image
    {
        public override Material GetModifiedMaterial(Material baseMaterial)
        {
            Material cModifiedMat = base.GetModifiedMaterial(baseMaterial);
          
            // Do whatever you want with this "cModifiedMat"...
            // You can also hold this and process it in your grayscale code.

            return cModifiedMat;
        }
       
    }
}
