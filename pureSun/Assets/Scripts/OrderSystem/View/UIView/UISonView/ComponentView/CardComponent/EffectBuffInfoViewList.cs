
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent
{
    public class EffectBuffInfoViewList : MonoBehaviour
    {
        public EffectBuffInfoView effectBuffInfoViewPrefab;

        private List<EffectBuffInfoView> effectBuffInfoViews = new List<EffectBuffInfoView>();

        public void LoadEffectBuffInfoList(List<EffectInfo> effectBuffInfoList)
        {
            for (int n = effectBuffInfoList.Count; n < effectBuffInfoViews.Count; n++)
            {
                effectBuffInfoViews[n].gameObject.SetActive(false);
            }
            for (int i = 0; i < effectBuffInfoList.Count; i++) {
                if (i < effectBuffInfoViews.Count)
                {
                    effectBuffInfoViews[i].gameObject.SetActive(true);
                    effectBuffInfoViews[i].LoadingEffectBuffInfoByEffectInfo(effectBuffInfoList[i]);
                }
                else {
                    EffectBuffInfoView effectBuffInfoView = Instantiate<EffectBuffInfoView>(effectBuffInfoViewPrefab);
                    Vector3 position = new Vector3();
                    effectBuffInfoView.transform.SetParent(transform, false);
                    effectBuffInfoView.transform.localPosition = position;
                    effectBuffInfoViews.Add(effectBuffInfoView);
                    effectBuffInfoView.LoadingEffectBuffInfoByEffectInfo(effectBuffInfoList[i]);
                }
            }
          
        }

    }
}
