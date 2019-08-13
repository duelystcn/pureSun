

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
                    //渲染费用栏
                    SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, null, UIViewSystemEvent.UI_MANA_INFA_SYS_OPEN);
                    //渲染科技栏
                    SendNotification(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS, null, UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_OPEN);

                    PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
                    QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;

                    circuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);

                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        //分发手牌
                        playerItem.DrawCard(5);
                        //设置起始费用上限
                        playerItem.manaItem.manaUpperLimit = 1;
                        //设置当前费用为0
                        playerItem.manaItem.manaUsable = 0;
                        
                        SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, playerItem.manaItem, StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_INIT,playerItem.playerCode));

                        SendNotification(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS, playerItem.traitCombination.traitTypes, StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_INIT, playerItem.playerCode));
                        //手牌渲染
                        //SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem, StringUtil.NotificationTypeAddPlayerCode(HandSystemEvent.HAND_CHANGE_AFFLUX, playerItem.playerCode));
                    }
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN);
                    break;
            }
        }
    }
}
