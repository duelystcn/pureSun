using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent
{
    public class EffectBuffInfoView : MonoBehaviour
    {
        public void LoadingEffectBuffInfoByEffectInfo( EffectInfo effectInfo) {
            TextMeshProUGUI effectBuffTitle = UtilityHelper.FindChild<TextMeshProUGUI>(transform, "EffectBuffTitle");
            effectBuffTitle.text = effectInfo.cardEntry.name;
            TextMeshProUGUI effectBuffText = UtilityHelper.FindChild<TextMeshProUGUI>(transform, "EffectBuffText");
            effectBuffText.text = effectInfo.description;

        }
    }
}
