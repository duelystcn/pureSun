
using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;
using static Assets.Scripts.OrderSystem.Model.Database.Card.CardEntry;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class ChooseStageCommand : SimpleCommand
    {


        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            ChooseStageCircuitProxy chooseStageCircuitProxy = Facade.RetrieveProxy(ChooseStageCircuitProxy.NAME) as ChooseStageCircuitProxy;
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_CHOOSE_STAGE_START:

                    List<CardInfo> shipCardList = cardDbProxy.GetCardInfoByType(CardMetrics.CARD_TYPE_SHIP);
                    //随机序列
                    List<int> randomList = RandomUtil.GetRandom(0,5,6,false);
                    List<string> playerCodeList = new List<string>();
                    foreach (string playerCode in playerGroupProxy.playerGroup.playerItems.Keys)
                    {
                        playerCodeList.Add(playerCode);
                    }
                    //为每个玩家分配不同的船
                    for (int rnum = 0;rnum<randomList.Count;rnum++) {
                        string playerCode = playerCodeList[rnum % playerCodeList.Count];
                        chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode].Add(shipCardList[randomList[rnum]]);
                    }
                    string playerCodeNow = chooseStageCircuitProxy.GetNowPlayerCode();
                    SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, null, UIViewSystemEvent.UI_CHOOSE_STAGE_OPEN);
                    SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCodeNow], UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD);
                    break;
                case UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD:
                    CardEntry card = notification.Body as CardEntry;
                    if (card.WhichCard == CardType.ShipCard)
                    {
                        string playerCode = chooseStageCircuitProxy.GetNowPlayerCode();
                        PlayerItem playerItemNow = playerGroupProxy.getPlayerByPlayerCode(playerCode);
                        playerItemNow.shipCard = card;
                        chooseStageCircuitProxy.IntoNextTurn();
                        playerCode = chooseStageCircuitProxy.GetNowPlayerCode();

                    }
                    else {

                    }
                    
                    break;
            }
        }
        
    }
}
