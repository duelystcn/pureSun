

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Controller
{
    class MinionSysCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            GameContainerProxy gameContainerProxy =
              Facade.RetrieveProxy(GameContainerProxy.NAME) as GameContainerProxy;
            PlayerGroupProxy playerGroupProxy =
              Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            switch (notification.Type)
            {
                //根据效果渲染高亮
                case MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT:
                    TargetSet targetSetToChoose = notification.Body as TargetSet;
                    //通知生物层发生变更重新渲染部分生物
                    SendNotification(MinionSystemEvent.MINION_VIEW, GetMinionListByTargetSet(targetSetToChoose, gameContainerProxy), MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE);
                    break;
                //根据效果渲染高亮-预定成为目标选择项
                case MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_BECOME_TARGET:
                    TargetSet targetSetToChooseTarget = notification.Body as TargetSet;
                    List<CardEntry> mList = GetMinionListByTargetSet(targetSetToChooseTarget, gameContainerProxy);

                    //通知生物层发生变更重新渲染部分生物
                    SendNotification(MinionSystemEvent.MINION_VIEW, mList, MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE_TO_CHOOSE_TARGET);
                    break;
                case MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE:
                    List<CardEntry> mListClose = new List<CardEntry>();
                    List<GameContainerItem> returnGameContainerItemList = gameContainerProxy.GetGameContainerItemGameContainerType("CardBattlefield");
                    foreach (GameContainerItem gameContainerItem in returnGameContainerItemList)
                    {
                        foreach (CardEntry minionCellItem in gameContainerItem.cardEntryList)
                        {
                            if (minionCellItem.IsEffectTarget == true)
                            {
                                minionCellItem.IsEffectTarget = false;
                            }
                            mListClose.Add(minionCellItem);
                        }
                    }
                    //通知生物层发生变更重新渲染部分生物
                    SendNotification(MinionSystemEvent.MINION_VIEW, mListClose, MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE);
                    break;
                //生物死亡
                case MinionSystemEvent.MINION_SYS_ONE_MINION_IS_DEAD:
                    CardEntry minionCellItemIsDead = notification.Body as CardEntry;
                    //放入墓地
                    minionCellItemIsDead.nextGameContainerType = "CardGraveyard";
                    minionCellItemIsDead.ttNeedChangeGameContainerType(minionCellItemIsDead);
                    break;

            }
        }

        public  List<CardEntry> GetMinionListByTargetSet (TargetSet targetSetToChoose, GameContainerProxy gameContainerProxy) {
            List<CardEntry> mList = new List<CardEntry>();
            List<GameContainerItem> returnGameContainerItemList = gameContainerProxy.GetGameContainerItemGameContainerType("CardBattlefield");
            foreach (GameContainerItem gameContainerItem in returnGameContainerItemList) {
                foreach (CardEntry minionCellItem in gameContainerItem.cardEntryList)
                {
                    if (targetSetToChoose.checkEffectToTargetMinionCellItem(minionCellItem))
                    {
                        UtilityLog.Log(" 检查了生物:" + minionCellItem.cardInfo.code, LogUtType.Special);
                        minionCellItem.IsEffectTarget = true;
                        mList.Add(minionCellItem);
                    }
                }
            }
            return mList;
        }
    }
}
