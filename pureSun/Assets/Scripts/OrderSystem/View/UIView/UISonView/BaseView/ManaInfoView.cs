
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System;
using TMPro;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class ManaInfoView : ViewBaseView
    {
        public TextMeshProUGUI myselfManaUpperLimit;
        public TextMeshProUGUI myselfManaUsable;
        public TextMeshProUGUI enmeyManaUpperLimit;
        public TextMeshProUGUI enmeyManaUsable;

        public void UIManaInfoSysInit(ManaItem manaItem, bool myself)
        {
            if (myself)
            {
                myselfManaUpperLimit.text = manaItem.manaUpperLimit.ToString();
                myselfManaUsable.text = manaItem.manaUsable.ToString();
            }
            else {
                enmeyManaUpperLimit.text = manaItem.manaUpperLimit.ToString();
                enmeyManaUsable.text = manaItem.manaUsable.ToString();
            }
           
        }

        public void ChangeManaUsable(int changeNum, bool myself)
        {
            if (myself) {
                myselfManaUsable.text = (Convert.ToInt32(myselfManaUsable.text) + changeNum).ToString();
            }
            else {
                enmeyManaUsable.text = (Convert.ToInt32(enmeyManaUsable.text) + changeNum).ToString();
            }
           
        }

        public void ChangeManaUpperLimit(int changeNum, bool myself)
        {
            if (myself)
            {
                myselfManaUpperLimit.text = (Convert.ToInt32(myselfManaUpperLimit.text) + changeNum).ToString();
            }
            else
            {
                enmeyManaUpperLimit.text = (Convert.ToInt32(enmeyManaUpperLimit.text) + changeNum).ToString();
            }

        }
    }
}
