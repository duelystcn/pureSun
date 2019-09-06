

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class QuestStageOneTurnCommand : SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN:
                    PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
                    //获取当前回合玩家，回合开始抽一张牌，费用恢复至上限
                    PlayerItem playerItem = playerGroupProxy.getPlayerByPlayerCode(circuitProxy.GetNowPlayerCode());
                    playerItem.RestoreToTheUpperLimit();
                    playerItem.DrawCard(1);
                    SendNotification(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS, null, StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS_SHOW, playerItem.playerCode));
                    break;
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_TRUN:
                    circuitProxy.IntoNextTurn();
                    break;
            }
        }
    }
}
