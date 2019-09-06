
using Assets.Scripts.OrderSystem.Common.UnityExpand;
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

        public void UIManaInfoSysInit(ManaItem manaItem, string playerCodeNotification)
        {
            manaUpperLimit.text = manaItem.manaUpperLimit.ToString();
            manaUsable.text = manaItem.manaUsable.ToString();
            playerCode = playerCodeNotification;
         

        }

        public void ChangeManaUsable(int changeNum)
        {
            manaUsable.text = (Convert.ToInt32(manaUsable.text) + changeNum).ToString();
        }

        public void ChangeManaUpperLimit(int changeNum)
        {
            manaUpperLimit.text = (Convert.ToInt32(manaUpperLimit.text) + changeNum).ToString();

        }
    }
}
