
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Assets.Scripts.OrderSystem.View.UIView.UIControllerListMediator;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.OperateComponent
{
    public class OneUserSelection : MonoBehaviour
    {
        public SendNotificationAddCardEntry sendNotification;

        public OneUserSelectionItem oneUserSelectionItem;

        public UnityAction choseThisView;

        public void PointerClick()
        {
            if (oneUserSelectionItem.defaultAvailab) {
                oneUserSelectionItem.cardEntry.needShowEffectInfo.needChoosePreEffect.playerDecisionLaunched = oneUserSelectionItem.isExecute;
                sendNotification(oneUserSelectionItem);
                choseThisView();
            }
            
        }


        public void LoadingInfoByOneUserSelection(OneUserSelectionItem oneUserSelectionItem, SendNotificationAddCardEntry sendNotificationAddCard, UnityAction choseThisView) {
            this.choseThisView = choseThisView;
            TextMeshProUGUI oneUserSelectionText = UtilityHelper.FindChild<TextMeshProUGUI>(this.transform, "OneUserSelectionText");
            oneUserSelectionText.text = oneUserSelectionItem.selectionText;
            MonoBehaviour oneUserSelectionBG = UtilityHelper.FindChild<MonoBehaviour>(this.transform, "OneUserSelectionBG");
            Image image = oneUserSelectionBG.transform.GetComponent<Image>();

            if (oneUserSelectionItem.defaultAvailab)
            {
                if (oneUserSelectionItem.isExecute)
                {
                    image.color = Color.green;
                }
                else {
                    image.color = Color.yellow;
                }
                this.sendNotification = sendNotificationAddCard;

            }
            else {
                image.color = Color.gray;
                this.sendNotification = (OneUserSelectionItem oneUserSelectionItemNull) => {
                
                };
            }
         
            this.oneUserSelectionItem = oneUserSelectionItem;
            
        }

    }
}
