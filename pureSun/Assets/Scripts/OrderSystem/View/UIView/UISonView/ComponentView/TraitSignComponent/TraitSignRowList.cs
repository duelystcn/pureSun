

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.TraitSignComponent
{
    //横向科技显示
    public class TraitSignRowList : ViewBaseView
    {
        public TraitSign traitSignPrefab;
        private List<TraitSign> traitSignlist = new List<TraitSign>();

        List<string> traitTypeList = new List<string>();

        //这个组件是属于谁的
        public string playerCode;

        public void UITraitTypeInit(List<TraitType> traitTypes) {
            traitTypeList = new List<string>();
            foreach (TraitType traitType in traitTypes) {
                traitTypeList.Add(traitType.ToString());
            }
            ReRenderTraitSignlist();
        }
        public void UITraitTypeAdd(string traitType) {
            traitTypeList.Add(traitType);
            ReRenderTraitSignlist();
        }
        public void ReRenderTraitSignlist() {
            //超出的设置为不启用
            if (traitSignlist.Count > traitTypeList.Count)
            {
                for (int n = traitTypeList.Count; n < traitSignlist.Count; n++)
                {
                    traitSignlist[n].gameObject.SetActive(false);
                }

            }

            for (int n = 0; n < traitTypeList.Count; n++)
            {
                TraitSign traitSign = null;
                bool isAdd = true;
                if (n < traitSignlist.Count)
                {
                    traitSignlist[n].gameObject.SetActive(true);
                    traitSign = traitSignlist[n];
                    isAdd = false;
                }
                else
                {
                    traitSign = Instantiate<TraitSign>(traitSignPrefab);
                    Vector3 position = new Vector3();
                    traitSign.transform.SetParent(transform, false);
                    traitSign.transform.localPosition = position;
                }
                if (isAdd)
                {
                    traitSignlist.Add(traitSign);
                }
                string path = "Image/Card/TraitSign/";
                path = path + traitTypeList[n] + "_trait";
                changeImageSprite(traitSign, path);
            }


        }
    }
}
