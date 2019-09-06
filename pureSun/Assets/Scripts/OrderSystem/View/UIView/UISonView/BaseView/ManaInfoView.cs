
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.ManaInfoComponent;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class ManaInfoView : ViewBaseView
    {
        public OnePlayerManaInfo myselfOnePlayerManaInfo;
        public OnePlayerManaInfo enemyOnePlayerManaInfo;

        public void UIManaInfoSysInit(ManaItem manaItem, bool myself, string playerCodeNotification)
        {
            if (myself)
            {
                myselfOnePlayerManaInfo.UIManaInfoSysInit(manaItem, playerCodeNotification);
            }
            else {
                enemyOnePlayerManaInfo.UIManaInfoSysInit(manaItem, playerCodeNotification);
            }
           
        }

        public void ChangeManaUsable(int changeNum, bool myself)
        {
            if (myself) {
                myselfOnePlayerManaInfo.ChangeManaUsable(changeNum);
            }
            else {
                enemyOnePlayerManaInfo.ChangeManaUsable(changeNum);
            }
           
        }

        public void ChangeManaUpperLimit(int changeNum, bool myself)
        {
            if (myself)
            {
                myselfOnePlayerManaInfo.ChangeManaUpperLimit(changeNum);
            }
            else
            {
                enemyOnePlayerManaInfo.ChangeManaUpperLimit(changeNum);
            }
        }
    }
}
