
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.ManaInfoComponent;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class ManaInfoView : ViewBaseView
    {
        public OnePlayerManaInfo myselfOnePlayerManaInfo;
        public OnePlayerManaInfo enemyOnePlayerManaInfo;

        public void UIManaInfoSysInit(VariableAttribute manaVariableAttribute, bool myself, string playerCodeNotification)
        {
            if (myself)
            {
                myselfOnePlayerManaInfo.UIManaInfoSysInit(manaVariableAttribute, playerCodeNotification);
            }
            else {
                enemyOnePlayerManaInfo.UIManaInfoSysInit(manaVariableAttribute, playerCodeNotification);
            }
           
        }

        public void ChangeManaUsable(VariableAttribute manaChangeVariableAttribute, bool myself)
        {
            if (myself) {
                myselfOnePlayerManaInfo.ChangeManaUsable(manaChangeVariableAttribute);
            }
            else {
                enemyOnePlayerManaInfo.ChangeManaUsable(manaChangeVariableAttribute);
            }
           
        }

       
    }
}
