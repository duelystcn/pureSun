using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
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

        public delegate void SendNotificationConfirmTargetMinion(CardEntry minionCellItem);


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

            List<CardEntry> mList = new List<CardEntry>();
            switch (notification.Name)
            {
                case MinionSystemEvent.MINION_VIEW:
                    switch (notification.Type) {
                        //生物模型变更，重新加载
                        //case MinionSystemEvent.MINION_VIEW_CHANGE_OVER:
                        //    MinionGridItem minionGridItem =  notification.Body as MinionGridItem;
                        //    minionGridView.AchieveMinionGrid(minionGridItem, hexGridProxy.HexGrid, this);
                        //    break;
                        case MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE:
                            mList = notification.Body as List<CardEntry>;
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE_TO_CHOOSE_TARGET:
                            mList = notification.Body as List<CardEntry>;
                            SendNotificationConfirmTargetMinion sendNotificationConfirmTargetMinion = (CardEntry minionCellItem) => {
                                SendNotification(UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS, null, UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS_TO_HIDE);
                                //取消渲染
                                SendNotification(MinionSystemEvent.MINION_SYS, null, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE);
                                SendNotification(OperateSystemEvent.OPERATE_SYS, minionCellItem, OperateSystemEvent.OPERATE_SYS_CHOOSE_ONE_MINION);
                            };
                            minionGridView.RenderSomeMinionByMinionCellItemToChooseTarget(mList, sendNotificationConfirmTargetMinion);
                            break;
                        case MinionSystemEvent.MINION_VIEW_ADD_ONE_MINION:
                            CardEntry minionCellItemAdd = notification.Body as CardEntry;
                            minionGridView.AchieveOneMinion(minionCellItemAdd, hexGridProxy.HexGrid, this);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MINION_CHANGE_ATTRIBUTE:
                            CardEntry minionCellItemChange = notification.Body as CardEntry;
                            mList.Add(minionCellItemChange);
                            minionGridView.RenderSomeMinionByMinionCellItem(mList);
                            break;
                       
                        case MinionSystemEvent.MINION_VIEW_ATTACK_TARGET_MINION:
                            callBackDelay = true;
                            CardEntry minionCellItemAttack = notification.Body as CardEntry;
                            UtilityLog.Log("玩家【" + minionCellItemAttack.controllerPlayerItem.playerCode + "】的生物【" + minionCellItemAttack.name + "】准备执行攻击动画", LogUtType.Attack);
                            callBack = () =>
                            {
                                exceINotification = false;
                                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EFFECT_SHOW_OVER);
                                SendNotification(MinionSystemEvent.MINION_VIEW, null, MinionSystemEvent.MINION_VIEW_ANIMATION_START);
                            };
                            minionGridView.MinionAttackTargetIndex(minionCellItemAttack, hexModelInfo, callBack);
                            break;
                        case MinionSystemEvent.MINION_VIEW_MOVE_TARGET_HEX_CELL:
                            CardEntry minionCellItemMove = notification.Body as CardEntry;
                            callBackSP = () =>
                            {
                                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EFFECT_SHOW_OVER);
                            };
                            minionGridView.MinionMoveTargetHexCell(minionCellItemMove, hexModelInfo, callBackSP);
                            break;
                        case MinionSystemEvent.MINION_VIEW_ONE_MINION_IS_DEAD:
                            CardEntry minionCellItemIsDead = notification.Body as CardEntry;
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
