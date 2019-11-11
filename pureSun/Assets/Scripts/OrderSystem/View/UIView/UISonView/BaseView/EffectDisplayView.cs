/*************************************************************************************
     * 类 名 称：       EffectDisplayView
     * 文 件 名：       EffectDisplayView
     * 创建时间：       2019-08-14
     * 作    者：       chenxi
     * 说   明：        用于展示正在释放的效果，实际上目前一次只会有一个效果被展示，
     *                  但是为了希望以后实现堆叠效果在这里使用了一个List来存放堆叠效果
     *                  
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.CardComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.EffectDisplayComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.ManaInfoComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.OperateComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.TraitSignComponent;
using OrderSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.View.UIView.UIControllerListMediator;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView
{
    public class EffectDisplayView : ViewBaseView
    {
        //为了堆叠准备的list
        public EffectShowList effectShowList;
        //目前只实现一次只显示一个效果结算引导
        public CardIntactView cardIntactView;

        //引导线，可能存在多个
        public List<EffectIndicationTrail> effectIndicationTrails = new List<EffectIndicationTrail>();

        //对象池
        public ObjectPool<EffectIndicationTrail> effectIndicationTrailPool;

        //用户选项表
        public UserSelectionList userSelectionList;

        


        protected override void InitUIObjects()
        {
            base.InitUIObjects();
            effectShowList.InitializationEffectShowList();
            cardIntactView.gameObject.SetActive(false);
            if (effectIndicationTrailPool == null) {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/OperateSystem/Trail/EffectIndicationTrail");
                effectIndicationTrailPool = new ObjectPool<EffectIndicationTrail>(prefab, "effectIndicationTrailPool");
            }
            
        }

        public void ShowCradEffectByCardEntry(CardEntry cardEntry, UnityAction callBack) {
            //添加需要展示的卡牌
            ShowCradEffect(cardEntry);
            //显示引导线
            ShowEffectIndicationTrail(cardEntry);
            //0.5秒后隐藏擦除
            StartCoroutine(ShowOverAction(callBack));
        }

        public void ShowCradEffectAndUserSelectionListByCardEntry(CardEntry cardEntry, SendNotificationAddCardEntry sendNotificationAddCard) {
            //添加需要展示的卡牌
            ShowCradEffect(cardEntry);
            ShowUserSelectListByCardEntry(cardEntry, sendNotificationAddCard);


        }
        public void ShowUserSelectListByCardEntry(CardEntry cardEntry, SendNotificationAddCardEntry sendNotificationAddCard) {
            //获取需要展示的效果
            EffectInfo needShowEffectInfo = cardEntry.needShowEffectInfo;
            //获取需要用户进行判断的效果
            EffectInfo needChoosePreEffect = needShowEffectInfo.needChoosePreEffect;
            List<OneUserSelectionItem> oneUserSelections = new List<OneUserSelectionItem>();
            //制作第一个选项,用户选择生效
            OneUserSelectionItem oneUserSelectionForYes = new OneUserSelectionItem();
            oneUserSelectionForYes.defaultAvailab = needChoosePreEffect.checkCanExecution;
            oneUserSelectionForYes.selectionText = needChoosePreEffect.description;
            oneUserSelectionForYes.isExecute = true;
            oneUserSelectionForYes.cardEntry = cardEntry;
            //制作第二个选项，用户选择不生效
            OneUserSelectionItem oneUserSelectionForNo = new OneUserSelectionItem();
            oneUserSelectionForNo.defaultAvailab = true;
            oneUserSelectionForNo.selectionText = "不执行";
            oneUserSelectionForNo.isExecute = false;
            oneUserSelectionForNo.cardEntry = cardEntry;
            oneUserSelections.Add(oneUserSelectionForYes);
            oneUserSelections.Add(oneUserSelectionForNo);
            //关闭窗口时执行的操作
            UnityAction choseThisView = () =>
            {
                  cardIntactView.gameObject.SetActive(false);
            };
            userSelectionList.LoadingUserSelectionListToCreate(oneUserSelections, sendNotificationAddCard, choseThisView);
        }

        public void ShowCradEffect(CardEntry cardEntry) {
            cardIntactView.gameObject.SetActive(true);
            cardIntactView.LoadCard(cardEntry,true);
        }
        public void ShowEffectIndicationTrail(CardEntry cardEntry)
        {
            List<Vector3> endVectors = new List<Vector3>();
            EffectInfo effectInfo = cardEntry.needShowEffectInfo;
            foreach (TargetSet targetSet in effectInfo.operationalTarget.selectTargetList) {
                switch (targetSet.target)
                {
                    case "Player":
                        for (int n = 0; n < targetSet.targetPlayerItems.Count; n++)
                        {
                            for (int m = 0; m < effectInfo.operationalContent.impactTargets.Length; m++)
                            {
                                OnePlayerManaInfo[] onePlayerManaInfos = null;
                                switch (effectInfo.operationalContent.impactTargets[m])
                                {
                                    //资源上限
                                    case "ManaUpperLimit":
                                        onePlayerManaInfos = GameObject.FindObjectsOfType<OnePlayerManaInfo>();
                                        foreach (PlayerItem playerItem in targetSet.targetPlayerItems)
                                        {
                                            foreach (OnePlayerManaInfo onePlayerManaInfo in onePlayerManaInfos)
                                            {
                                                if (onePlayerManaInfo.playerCode == playerItem.playerCode)
                                                {
                                                    Vector3 endPosition = onePlayerManaInfo.transform.position;
                                                    endPosition.y = endPosition.y + 5;
                                                    endVectors.Add(endPosition);
                                                }
                                            }
                                        }
                                        break;
                                    //可用费用
                                    case "ManaUsable":
                                        onePlayerManaInfos = GameObject.FindObjectsOfType<OnePlayerManaInfo>();
                                        foreach (PlayerItem playerItem in targetSet.targetPlayerItems)
                                        {
                                            foreach (OnePlayerManaInfo onePlayerManaInfo in onePlayerManaInfos)
                                            {
                                                if (onePlayerManaInfo.playerCode == playerItem.playerCode)
                                                {
                                                    Vector3 endPosition = onePlayerManaInfo.transform.position;
                                                    endPosition.y = endPosition.y + 5;
                                                    endVectors.Add(endPosition);
                                                }
                                            }
                                        }
                                        break;
                                    //科技相关
                                    case "TraitAddOne":
                                        TraitSignRowList[] traitSignRowLists = GameObject.FindObjectsOfType<TraitSignRowList>();
                                        foreach (PlayerItem playerItem in targetSet.targetPlayerItems)
                                        {
                                            foreach (TraitSignRowList traitSignRowList in traitSignRowLists)
                                            {
                                                if (traitSignRowList.playerCode == playerItem.playerCode)
                                                {
                                                    Vector3 endPosition = traitSignRowList.transform.position;
                                                    endPosition.y = endPosition.y + 5;
                                                    endVectors.Add(endPosition);
                                                }
                                            }
                                        }
                                        break;
                                    //分数相关
                                    case "Score":
                                        ShipComponentView[] shipComponentViews = GameObject.FindObjectsOfType<ShipComponentView>();
                                        foreach (PlayerItem playerItem in targetSet.targetPlayerItems)
                                        {
                                            if (shipComponentViews[0].myselfPlayerCode == playerItem.playerCode)
                                            {
                                                Vector3 endPosition = shipComponentViews[0].myselfScore.transform.position;
                                                endPosition.y = endPosition.y + 5;
                                                endVectors.Add(endPosition);
                                            }
                                            else
                                            {
                                                Vector3 endPosition = shipComponentViews[0].enemyScore.transform.position;
                                                endPosition.y = endPosition.y + 5;
                                                endVectors.Add(endPosition);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;


                }

            }
            //起始点固定
            Vector3 startPosition = cardIntactView.transform.position;
            startPosition.y = startPosition.y + 5;

            foreach (Vector3 endPosition in endVectors) {
                EffectIndicationTrail effectIndicationTrail = effectIndicationTrailPool.Pop();

                effectIndicationTrail.transform.SetParent(transform, false);
                effectIndicationTrail.transform.localPosition = new Vector3(0, 0, 0);
                Vector3[] positions = new Vector3[2];
                positions[0] = startPosition;
                positions[1] = endPosition;
                effectIndicationTrail.changePositions(positions);
                effectIndicationTrails.Add(effectIndicationTrail);

            }

          
        }
        public IEnumerator ShowOverAction(UnityAction callBack)
        {
            yield return new WaitForSeconds(1f);
            cardIntactView.gameObject.SetActive(false);
            for (int n = 0; n < effectIndicationTrails.Count; n++) {
                effectIndicationTrailPool.Push(effectIndicationTrails[n]);
            }

            callBack();
        }

    }
}
