

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Assets.Scripts.OrderSystem.Controller
{
    public class QuestStageCommand : SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_QUEST_STAGE_START:
                    //通知渲染战场
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_SYS_SHOW_START);

                    PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
                    QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;

                    circuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);

                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        //分发手牌
                        playerItem.DrawCard(4);
                        //设置起始费用上限
                        playerItem.manaItem.manaUpperLimit = 0;


                        //手牌渲染
                        //SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem, StringUtil.NotificationTypeAddPlayerCode(HandSystemEvent.HAND_CHANGE_AFFLUX, playerItem.playerCode));
                    }  
                    
                    break;
            }
        }
    }
}
