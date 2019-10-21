

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.Animation
{
    public class CardMoveAnimation : ViewBaseView
    {
        public CardIntactView cardIntactViewPrefab;
        //用于做动画的卡，暂时先用这个吧
        private CardIntactView cardMoveAnimation;


        public void MoveShipCardAnimation(CardIntactView cardIntactView, CardDeckList cardDeckList, UnityAction callBack)
        {
            //获取当前位置
            Vector3 startPosition = cardIntactView.transform.position;
            Vector3 endPosition = cardDeckList.transform.position;
            //激活动画组件
            ActiveCardMoveAnimation(startPosition);
            cardMoveAnimation.LoadCard(cardIntactView.card,true);
            StartCoroutine(MoveOneCard(callBack, startPosition, endPosition));
        }

        //移动某个子节点到某个位置
        public IEnumerator MoveOneCard(UnityAction callBack, Vector3 startPosition, Vector3 endPosition)
        {
           
          
            bool isNear = false;
            Vector3 position = new Vector3();
            position.y = 0;
            int xdirection = endPosition.x - startPosition.x == 0 ? 0 : endPosition.x - startPosition.x > 0 ? 1 : -1;
            int zdirection = endPosition.z - startPosition.z == 0 ? 0 : endPosition.z - startPosition.z > 0 ? 1 : -1;

            position.x = 8 * xdirection;
            position.z = 8 * zdirection;

            while (!isNear)
            {
                cardMoveAnimation.transform.Translate(position * Time.deltaTime);
                if (Math.Abs(cardMoveAnimation.transform.position.x - endPosition.x) < 0.5)
                {
                    if (Math.Abs(cardMoveAnimation.transform.position.z - endPosition.z) < 0.5)
                    {
                        isNear = true;
                    }
                }
                yield return null;
            }
            CloseCardMoveAnimation();
            callBack();

        }

        //激活动画组件
        public void ActiveCardMoveAnimation(Vector3 startPosition)
        {
            if (cardMoveAnimation == null)
            {
                cardMoveAnimation = Instantiate<CardIntactView>(cardIntactViewPrefab);
                cardMoveAnimation.transform.SetParent(transform, false);
            }
            cardMoveAnimation.gameObject.SetActive(true);
            cardMoveAnimation.transform.localPosition = startPosition;
           

        }
        //关闭动画组件
        public void CloseCardMoveAnimation()
        {
            cardMoveAnimation.gameObject.SetActive(false);

        }

    }
}
