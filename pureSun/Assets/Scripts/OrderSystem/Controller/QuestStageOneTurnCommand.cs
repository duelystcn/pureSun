

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
            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN:
                    QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
                    PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
                    //获取当前回合玩家，回合开始抽一张牌，费用恢复至上限
                    PlayerItem playerItem = playerGroupProxy.getPlayerByPlayerCode(circuitProxy.GetNowPlayerCode());
                    playerItem.RestoreToTheUpperLimit();
                    playerItem.DrawCard(1);
                    break;
            }
        }
    }
}
