

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.TraitSignComponent
{
    public class TraitSignList : ViewBaseView
    {

        public TraitSign traitSignPrefab;
        private List<TraitSign> traitSignlist = new List<TraitSign>();
        public void LoadTraitList(string[] traitdemand) {
            //超出的设置为不启用
            if (traitSignlist.Count > traitdemand.Length) {
                for (int n = traitdemand.Length; n < traitSignlist.Count; n++) {
                    traitSignlist[n].gameObject.SetActive(false);
                }

            }
           
            for (int n = 0; n < traitdemand.Length; n++)
            {
                TraitSign traitSign = null;
                bool isAdd = true;
                if (n < traitSignlist.Count)
                {
                    traitSignlist[n].gameObject.SetActive(true);
                    traitSign = traitSignlist[n];
                    isAdd = false;
                }
                else {
                    traitSign = Instantiate<TraitSign>(traitSignPrefab);
                    Vector3 position = new Vector3();
                    traitSign.transform.SetParent(transform, false);
                    traitSign.transform.localPosition = position;
                }
                if (isAdd) {
                    traitSignlist.Add(traitSign);
                }
                string path = "Image/Card/TraitSign/";
                path = path + traitdemand[n] + "_trait";
                changeImageSprite(traitSign, path);
            }
         
        }

    }
}
