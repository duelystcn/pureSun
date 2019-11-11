using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.MinionView
{
    public class MinionGridMediator : MediatorExpand
    {
        public new const string NAME = "MinionGridMediator";


        private HexGridProxy hexGridProxy = null;


        public MinionGridView minionGridView
        {
            get { return (MinionGridView)base.ViewComponent; }
        }

        public MinionGridMediator(MinionGridView minionGridView) : base(NAME, minionGridView)
        {
        }

        //注册时执行
        public override void OnRegister()
        {
            base.OnRegister();
            hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
        }
        //监听
        public override string[] ListNotificationInterests()
        {
            List<string> notificationList = new List<string>();
            notificationList.Add(MinionSystemEvent.MINION_VIEW);
            AddCommonNotificationInterests(notificationList);
            return notificationList.ToArray();
        }
        public override void HandleNotification(INotification notification)
        {

            if (notification.Name == MinionSystemEvent.MINION_VIEW && notification.Type == MinionSystemEvent.MINION_VIEW_ANIMATION_START)
            {
                DoExceHandleNotification();
            }
            else if (notification.Name == OrderSystemEvent.CLINET_SYS)
            {
                //客户端监听发放，不做处理
                HandleNotificationCommon(notification);
            }
            else
            {
                notificationQueue.Enqueue(notification);
                DoExceHandleNotification();
            }

        }

        public override void ExceHandleNotification(INotification notification)
        {

            //处理公共请求
            HandleNotificationCommon(notification);
            //回调函数
            UnityAction callBack = () =>
            {
                exceINotification = false;
                SendNotification(MinionSystemEvent.MINION_VIEW, null, MinionSystemEvent.MINION_VIEW_ANIMATION_START);

            };
            UnityAction callBackSP = () => { };
            bool callBackDelay = false;

            List<MinionCellItem> mList = new List<MinionCellItem>();
            switch (notification.Name)
            {
                case MinionSystemEvent.MINION_VIEW:
                    switch (notification.Type) {
                        //生物模型变更，重新加载
                        case MinionSystemEvent.MINION_VIEW_CHANGE_OVER:
                            MinionGridItem minionGridItem =  notification.Body as MinionGridItem;
                            minionGridView.AchieveMinionGrid(minionGridItem, hexGridProxy.HexGrid, this);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE:
                            mList = notification.Body as List<MinionCellItem>;
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                        case MinionSystemEvent.MINION_VIEW_ADD_ONE_MINION:
                            MinionCellItem minionCellItemAdd = notification.Body as MinionCellItem;
                            minionGridView.AchieveOneMinion(minionCellItemAdd, hexGridProxy.HexGrid, this);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINION_CHANGE_ATK:
                            MinionCellItem minionCellItemAtk = notification.Body as MinionCellItem;
                            mList.Add(minionCellItemAtk);
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINION_CHANGE_DEF:
                            MinionCellItem minionCellItemDef = notification.Body as MinionCellItem;
                            mList.Add(minionCellItemDef);
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                        case MinionSystemEvent.MINION_VIEW_ATTACK_TARGET_MINION:
                            callBackDelay = true;
                            MinionCellItem minionCellItemAttack = notification.Body as MinionCellItem;
                            UtilityLog.Log("玩家【" + minionCellItemAttack.controllerPlayerItem.playerCode + "】的生物【" + minionCellItemAttack.cardEntry.name + "】准备执行攻击动画", LogUtType.Attack);
                            callBack = () =>
                            {
                                exceINotification = false;
                                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EFFECT_SHOW_OVER);
                                SendNotification(MinionSystemEvent.MINION_VIEW, null, MinionSystemEvent.MINION_VIEW_ANIMATION_START);
                            };
                            minionGridView.MinionAttackTargetIndex(minionCellItemAttack, hexModelInfo, callBack);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MOVE_TARGET_HEX_CELL:
                            MinionCellItem minionCellItemMove = notification.Body as MinionCellItem;
                            callBackSP = () =>
                            {
                                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EFFECT_SHOW_OVER);
                            };
                            minionGridView.MinionMoveTargetHexCell(minionCellItemMove, hexModelInfo, callBackSP);
                            break;
                        case MinionSystemEvent.MINION_VIEW_ONE_MINION_IS_DEAD:
                            MinionCellItem minionCellItemIsDead = notification.Body as MinionCellItem;
                            minionGridView.MinionIsDeadNeedRemove(minionCellItemIsDead);
                            break;

                    }
                    break;
               

            }
            if (callBackDelay == false)
            {
                callBack();
            }
        }
    }
}
