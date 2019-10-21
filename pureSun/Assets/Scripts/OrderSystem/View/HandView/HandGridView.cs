using Assets.Scripts.OrderSystem.Common.UnityExpand;
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

        public void AchieveHandGrid(HandGridItem handGridItem) {
            //清除
            foreach (HandCellView handCellView in handCellViews)
            {
                handCellPool.Push(handCellView);
            }
            //重新
            for (int i = 0; i < handGridItem.handCells.Count(); i++)
            {
                HandCellItem handCellItem = handGridItem.handCells[i];
                Vector3 position = new Vector3();
                //position = HandMetrics.erectPosition(position, handCellItem.X);
                //创建一个格子实例
                //       HandCellView cell = Instantiate<HandCellView>(cellPrefab);
                HandCellView cell = handCellPool.Pop();
                //设置图片
                //cell.GetComponent<HandCellInstance>().SetImage();
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = position;
                cell.LoadHandCellItem(handCellItem);
                handCellViews.Add(cell);
            }
                //渲染需要放在格子生成完毕后
                //不再渲染，改为用贴图
                //handMesh.Triangulate(handCellViews);
        }
        

        //鼠标移入了某一张卡牌
        public void OneCardMousenPointerEnter(HandCellItem handCellItem) {
            foreach (HandCellView handCellView in handCellViews)
            {
                if (handCellView.handCellItem.uuid == handCellItem.uuid)
                {
                    
                }
            }
        }
        //鼠标移出了某一张卡牌
        public void OneCardMousenPointerExit(HandCellItem handCellItem){
            foreach (HandCellView handCellView in handCellViews)
            {
                if (handCellView.handCellItem.uuid == handCellItem.uuid)
                {
                   
                }
            }


        }
        //改变了可用显示
        public void HandChangeCanUseJudge(List<HandCellItem> handCellItems) {
            foreach (HandCellItem handCellItem in handCellItems) {
                foreach (HandCellView handCellView in handCellViews) {
                    if (handCellItem.cardEntry.uuid == handCellView.handCellItem.cardEntry.uuid) {
                        handCellView.SetCanUseOutLight(handCellItem);
                    }

                }
            }
        }
        //选中的手牌没有被成功使用
        public void HandChangeUncheckHandItem(HandCellItem uncheckHandCellItem)
        {
            foreach (HandCellView handCellView in handCellViews)
            {
                if (uncheckHandCellItem.cardEntry.uuid == handCellView.handCellItem.cardEntry.uuid)
                {
                    handCellView.UncheckChange();
                }

            }  
        }


        //添加一张牌
        public void PlayerDrawOneCard(HandCellItem handCellItem, UnityAction callBack)
        {
            StartCoroutine(PlayerDrawOneCardEnumerator(handCellItem , callBack));
        }
        //添加一张牌的动画
        public IEnumerator PlayerDrawOneCardEnumerator(HandCellItem handCellItem, UnityAction callBack)
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
        public void PlayerRemoveOneCard(HandCellItem handCellItem, UnityAction callBack)
        {
            //清除
            foreach (HandCellView handCellView in handCellViews)
            {
                if (handCellView.handCellItem.uuid == handCellItem.uuid)
                {
                    handCellPool.Push(handCellView);
                    break;
                }
            }
            callBack();
        }

    }
}
