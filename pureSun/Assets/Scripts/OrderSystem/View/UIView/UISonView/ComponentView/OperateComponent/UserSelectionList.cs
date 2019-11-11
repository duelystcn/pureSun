

using Assets.Scripts.OrderSystem.Model.OperateSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.View.UIView.UIControllerListMediator;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.OperateComponent
{
    public class UserSelectionList : MonoBehaviour
    {
        public OneUserSelection oneUserSelectionPrefab;

        public List<OneUserSelection> oneUserSelectionList = new List<OneUserSelection>();


        public void LoadingUserSelectionListToCreate(List<OneUserSelectionItem> osParameterList, SendNotificationAddCardEntry sendNotificationAddCard, UnityAction choseThisView)
        {
            choseThisView += () =>
            {
                choseThisList();
            };
            for (int n = 0; n < osParameterList.Count; n++)
            {
                if (n < oneUserSelectionList.Count)
                {
                    oneUserSelectionList[n].gameObject.SetActive(true);
                    oneUserSelectionList[n].LoadingInfoByOneUserSelection(osParameterList[n], sendNotificationAddCard, choseThisView);
                }
                else
                {
                    OneUserSelection cardIntactView = Instantiate<OneUserSelection>(oneUserSelectionPrefab);
                    Vector3 position = new Vector3();
                    cardIntactView.transform.SetParent(transform, false);
                    cardIntactView.transform.localPosition = position;
                    cardIntactView.LoadingInfoByOneUserSelection(osParameterList[n], sendNotificationAddCard, choseThisView);
                    oneUserSelectionList.Add(cardIntactView);
                }
            }
            for (int m = osParameterList.Count; m < oneUserSelectionList.Count; m++)
            {
                oneUserSelectionList[m].gameObject.SetActive(false);
            }
        }

        public void choseThisList() {
            for (int m = 0; m < oneUserSelectionList.Count; m++)
            {
                oneUserSelectionList[m].gameObject.SetActive(false);
            }

        }

    }
}
