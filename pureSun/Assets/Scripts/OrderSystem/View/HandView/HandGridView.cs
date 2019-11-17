using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using OrderSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.HandView
{
    public class HandGridView : MonoBehaviour
    {
        public List<HandCellView> handCellViews;
        //对象池
        public ObjectPool<HandCellView> handCellPool;
        public HandCellView cellPrefab;
        HandMesh handMesh;
        //显示的是对手还是自己
        public bool myself;

       
       
        void Awake()
        {
            handMesh = GetComponentInChildren<HandMesh>();
            //Resources
            GameObject prefab = Resources.Load<GameObject>("Prefabs/HandCell/HandCell");
            handCellPool = new ObjectPool<HandCellView>(prefab, "handCellPool");
            handCellViews = new List<HandCellView>();
        }

        public void AchieveHandGrid(GameContainerItem handGridItem, bool myself) {
            //清除
            foreach (HandCellView handCellView in handCellViews)
            {
                handCellPool.Push(handCellView);
            }
            //重新
            for (int i = 0; i < handGridItem.cardEntryList.Count(); i++)
            {
                CardEntry handCellItem = handGridItem.cardEntryList[i];
                Vector3 position = new Vector3();
                //position = HandMetrics.erectPosition(position, handCellItem.X);
                //创建一个格子实例
                //       HandCellView cell = Instantiate<HandCellView>(cellPrefab);
                HandCellView cell = handCellPool.Pop();
                //设置图片
                //cell.GetComponent<HandCellInstance>().SetImage();
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = position;
                cell.myself = myself;
                cell.LoadHandCellItem(handCellItem);
                handCellViews.Add(cell);
            }
                //渲染需要放在格子生成完毕后
                //不再渲染，改为用贴图
                //handMesh.Triangulate(handCellViews);
        }
        

        //鼠标移入了某一张卡牌
        public void OneCardMousenPointerEnter(CardEntry handCellItem) {
            foreach (HandCellView handCellView in handCellViews)
            {
                if (handCellView.handCellItem.uuid == handCellItem.uuid)
                {
                    
                }
            }
        }
        //鼠标移出了某一张卡牌
        public void OneCardMousenPointerExit(CardEntry handCellItem){
            foreach (HandCellView handCellView in handCellViews)
            {
                if (handCellView.handCellItem.uuid == handCellItem.uuid)
                {
                   
                }
            }


        }
        //改变了可用显示
        public void HandChangeCanUseJudge(List<CardEntry> handCellItems) {
            foreach (CardEntry handCellItem in handCellItems) {
                foreach (HandCellView handCellView in handCellViews) {
                    if (handCellItem.uuid == handCellView.handCellItem.uuid) {
                        handCellView.SetCanUseOutLight(handCellItem);
                    }

                }
            }
        }
        //选中的手牌没有被成功使用
        public void HandChangeUncheckHandItem(CardEntry uncheckHandCellItem)
        {
            foreach (HandCellView handCellView in handCellViews)
            {
                if (uncheckHandCellItem.uuid == handCellView.handCellItem.uuid)
                {
                    handCellView.UncheckChange();
                }

            }  
        }


        //添加一张牌
        public void PlayerDrawOneCard(CardEntry handCellItem, UnityAction callBack)
        {
            StartCoroutine(PlayerDrawOneCardEnumerator(handCellItem , callBack));
        }
        //添加一张牌的动画
        public IEnumerator PlayerDrawOneCardEnumerator(CardEntry handCellItem, UnityAction callBack)
        {
            Vector3 position = new Vector3();
            //position = HandMetrics.erectPosition(position, handCellItem.X);
            //创建一个格子实例
            //       HandCellView cell = Instantiate<HandCellView>(cellPrefab);
            HandCellView cell = handCellPool.Pop();
            //设置图片
            //cell.GetComponent<HandCellInstance>().SetImage();
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.myself = myself;
            cell.LoadHandCellItem(handCellItem);
            handCellViews.Add(cell);
            yield return new WaitForSeconds(0.25f);
            callBack();

        }
        //移除一张牌
        public void PlayerRemoveOneCard(CardEntry handCellItem, UnityAction callBack)
        {
            //清除
            int needRemoveNum = -1;
            for (int num = 0; num < handCellViews.Count; num++) {
                HandCellView handCellView = handCellViews[num];
                if (handCellView.handCellItem.uuid == handCellItem.uuid)
                {
                    needRemoveNum = num;
                    handCellPool.Push(handCellView);
                    break;
                }

            }
            if (needRemoveNum > -1) {
                handCellViews.RemoveAt(needRemoveNum);
            }
            callBack();
        }
        //隐藏一张牌
        public void HideOneCard(CardEntry handCellItem, UnityAction callBack)
        {
            //清除
            foreach (HandCellView handCellView in handCellViews)
            {
                if (handCellView.handCellItem.uuid == handCellItem.uuid)
                {
                    handCellView.gameObject.SetActive(false);
                    break;
                }
            }
            callBack();
        }

    }
}
