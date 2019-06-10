

using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Minion;
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
            switch (notification.Type)
            {
                //根据效果渲染高亮
                case MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT:
                    EffectInfo effectInfo = notification.Body as EffectInfo;
                    List<MinionCellItem> mList = new List<MinionCellItem>();
                    foreach (MinionCellItem minionCellItem in minionGridProxy.minionGridItem.minionCells.Values) {
                        minionCellItem.IsHighLight = true;
                        mList.Add(minionCellItem);
                    }
                    //通知生物层发生变更重新渲染部分生物
                    SendNotification(MinionSystemEvent.MINION_VIEW, mList, MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE);
                    break;
                case MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE:
                    List<MinionCellItem> mListClose = new List<MinionCellItem>();
                    foreach (MinionCellItem minionCellItem in minionGridProxy.minionGridItem.minionCells.Values)
                    {
                        if (minionCellItem.IsHighLight == true) {
                            minionCellItem.IsHighLight = false;
                        }
                        mListClose.Add(minionCellItem);
                    }
                    //通知生物层发生变更重新渲染部分生物
                    SendNotification(MinionSystemEvent.MINION_VIEW, mListClose, MinionSystemEvent.MINION_VIEW_MINIONS_CHANGE);
                    break;

            }
        }
    }
}
