
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System;
using TMPro;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.ManaInfoComponent
{
    public class OnePlayerManaInfo : ViewBaseView
    {
        public TextMeshProUGUI manaUpperLimit;
        public TextMeshProUGUI manaUsable;

        public string playerCode;

        public void UIManaInfoSysInit(VariableAttribute manaVariableAttribute, string playerCodeNotification)
        {
            manaUpperLimit.text = (manaVariableAttribute.valueMap[VATtrtype.OriginalValue]).ToString();
            manaUsable.text = (manaVariableAttribute.valueMap[VATtrtype.OriginalValue] + manaVariableAttribute.valueMap[VATtrtype.ChangeValue] + manaVariableAttribute.valueMap[VATtrtype.DamageValue]).ToString();
            playerCode = playerCodeNotification;
         

        }

        public void ChangeManaUsable(VariableAttribute manaVariableAttribute)
        {
            manaUpperLimit.text = (manaVariableAttribute.valueMap[VATtrtype.OriginalValue]).ToString();
            manaUsable.text = (manaVariableAttribute.valueMap[VATtrtype.OriginalValue] + manaVariableAttribute.valueMap[VATtrtype.ChangeValue] + manaVariableAttribute.valueMap[VATtrtype.DamageValue]).ToString();
        }

       
    }
}
