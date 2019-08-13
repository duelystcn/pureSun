using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    public class TimeTriggerSysCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            switch (notification.Type)
            {
                case TimeTriggerEvent.TIME_TRIGGER_SYS_HAND_CAN_USE_JUDGE:
                    string playerCode = notification.Body as string;
                    PlayerItem playerItem = playerGroupProxy.playerGroup.playerItems[playerCode];
                    //当前可用费用
                    int manaUsable = playerItem.manaItem.manaUsable;
                    //当前科技
                    List<TraitType> traitTypes = playerItem.traitCombination.traitTypes;
                    foreach (HandCellItem handCellItem in playerItem.handGridItem.handCells) {
                        bool canUse = true;
                        //检查费用
                        if (handCellItem.cardEntry.cost > manaUsable) {
                            canUse = false;
                        }
                        //检查科技要求
                        HashSet<string> traitdemandSet = new HashSet<string>(handCellItem.cardEntry.traitdemand);
                        foreach (string traitdemandNeed in traitdemandSet) {
                            //需求是多少个
                            int traitdemandNum = 0;
                            foreach (string traitdemand in handCellItem.cardEntry.traitdemand)
                            {
                                if (traitdemand == traitdemandNeed) {
                                    traitdemandNum++;
                                }
                            }
                            //目前的科技有多少
                            int traitTypeNum = 0;
                            foreach (TraitType traitType in traitTypes)
                            {
                                if (traitType.ToString() == traitdemandNeed)
                                {
                                    traitTypeNum++;
                                }
                            }
                            if (traitdemandNum > traitTypeNum) {
                                canUse = false;
                            }
                        }
                        handCellItem.canUse = canUse;
                    }
                    SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem.handCells, HandSystemEvent.HAND_CHANGE_CAN_USE_JUDGE);
                    break;
            }
        }
    }
}
