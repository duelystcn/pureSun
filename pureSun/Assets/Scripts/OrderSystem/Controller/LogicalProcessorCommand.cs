

using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Assets.Scripts.OrderSystem.Controller
{
    //我希望把所有需要判断操作的都放到这里，来判断玩家操作和AI操作
    public class LogicalProcessorCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            ChooseStageCircuitProxy chooseStageCircuitProxy = Facade.RetrieveProxy(ChooseStageCircuitProxy.NAME) as ChooseStageCircuitProxy;
            switch (notification.Type)
            {
                case LogicalSysEvent.LOGICAL_SYS_CHOOSE_SHIP_CARD:
                    string playerCode = notification.Body as string;
                    PlayerItem playerItem = playerGroupProxy.getPlayerByPlayerCode(playerCode);
                    if (playerItem.playerType == PlayerType.HumanPlayer)
                    {
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode], UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_INFO);

                    }
                    else {
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode], UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_INFO);
                    }
                    break;
            }
        }
    }
}
