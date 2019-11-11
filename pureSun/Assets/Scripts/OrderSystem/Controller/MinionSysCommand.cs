﻿

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
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
            MinionGridProxy minionGridProxy =
               Facade.RetrieveProxy(MinionGridProxy.NAME) as MinionGridProxy;
            PlayerGroupProxy playerGroupProxy =
              Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            switch (notification.Type)
            {
                //根据效果渲染高亮
                case MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT:
                    TargetSet targetSetToChoose = notification.Body as TargetSet;
                    List<MinionCellItem> mList = new List<MinionCellItem>();
                    foreach (MinionCellItem minionCellItem in minionGridProxy.minionGridItem.minionCells.Values) {
                        if (targetSetToChoose.checkEffectToTargetMinionCellItem(minionCellItem)) {
                            minionCellItem.IsEffectTarget = true;
                            mList.Add(minionCellItem);
                        } 
                    }
                    //通知生物层发生变更重新渲染部分生物
                    SendNotification(MinionSystemEvent.MINION_VIEW, mList, MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE);
                    break;
                case MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE:
                    List<MinionCellItem> mListClose = new List<MinionCellItem>();
                    foreach (MinionCellItem minionCellItem in minionGridProxy.minionGridItem.minionCells.Values)
                    {
                        if (minionCellItem.IsEffectTarget == true) {
                            minionCellItem.IsEffectTarget = false;
                        }
                        mListClose.Add(minionCellItem);
                    }
                    //通知生物层发生变更重新渲染部分生物
                    SendNotification(MinionSystemEvent.MINION_VIEW, mListClose, MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE);
                    break;
                //生物死亡
                case MinionSystemEvent.MINION_SYS_ONE_MINION_IS_DEAD:
                    MinionCellItem minionCellItemIsDead = notification.Body as MinionCellItem;
                    minionGridProxy.minionGridItem.minionCells.Remove(minionCellItemIsDead.index);
                    //放入墓地
                    minionCellItemIsDead.cardEntry.nextGameContainerType = "CardGraveyard";
                    minionCellItemIsDead.cardEntry.ttNeedChangeGameContainerType(minionCellItemIsDead.cardEntry);
                    break;

            }
        }
    }
}
