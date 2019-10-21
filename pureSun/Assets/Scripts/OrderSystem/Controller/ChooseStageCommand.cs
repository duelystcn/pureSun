
using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;
using static Assets.Scripts.OrderSystem.Model.Database.Card.CardEntry;

namespace Assets.Scripts.OrderSystem.Controller
{
    //选择船只界面
    internal class ChooseStageCommand : SimpleCommand
    {


        public override void Execute(INotification notification)
        {
           
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            ChooseStageCircuitProxy chooseStageCircuitProxy = Facade.RetrieveProxy(ChooseStageCircuitProxy.NAME) as ChooseStageCircuitProxy;
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            //当前回合玩家
            string playerCodeChoose = chooseStageCircuitProxy.GetNowPlayerCode();
            PlayerItem playerItemNow = playerGroupProxy.getPlayerByPlayerCode(playerCodeChoose);

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
                        CardEntry fpCardShip = new CardEntry();
                        fpCardShip.InitializeByCardInfo(shipCardList[randomList[rnum]]);
                        chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode].Add(fpCardShip);
                    }
                    string playerCodeNow = chooseStageCircuitProxy.GetNowPlayerCode();
                    SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, null, StringUtil.GetNTByNotificationTypeAndUIViewName(UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW, UIViewConfig.getNameStrByUIViewName(UIViewName.ChooseStage)));
                    SendNotification(LogicalSysEvent.LOGICAL_SYS, playerCodeNow, LogicalSysEvent.LOGICAL_SYS_CHOOSE_SHIP_CARD);
                    break;
                case UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD:
                    CardEntry card = notification.Body as CardEntry;
                   
                    if (card.WhichCard == CardType.ShipCard)
                    {
                       //确认选择，然后返回选择动画
                        playerItemNow.shipCard = card;
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, card, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_SHIP_CARD_ANIMATION, playerItemNow.playerCode));
                        SendNotification(UIViewSystemEvent.UI_CARD_DECK_LIST, playerItemNow, UIViewSystemEvent.UI_CARD_DECK_LIST_LOAD);

                        chooseStageCircuitProxy.IntoNextTurn();
                        playerCodeChoose = chooseStageCircuitProxy.GetNowPlayerCode();

                        //查看是否所有玩家都选择了船
                        if (playerGroupProxy.checkAllPlayerHasShip())
                        {
                            //渲染卡池
                            // List<CardEntry> cardEntries = cardDbProxy.GetSameCardEntry(3);
                            SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, UIViewConfig.getNameStrByUIViewName(UIViewName.ChooseStage), UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW);
                            //第一次发牌，发三组
                            List<List<CardEntry>> cardEntries = new List<List<CardEntry>>();
                            cardEntries.Add(cardDbProxy.GetOneCardListForPool());
                            cardEntries.Add(cardDbProxy.GetOneCardListForPool());
                            cardEntries.Add(cardDbProxy.GetOneCardListForPool());

                            SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, cardEntries, StringUtil.GetNTByNotificationTypeAndUIViewName(UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW, UIViewConfig.getNameStrByUIViewName(UIViewName.ViewChooseMakeStage)));
                            //SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, cardEntries, UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY);
                        }
                        else
                        {

                            //渲染下一位玩家的船只选择
                            SendNotification(LogicalSysEvent.LOGICAL_SYS, playerCodeChoose, LogicalSysEvent.LOGICAL_SYS_CHOOSE_SHIP_CARD);

                        }
                    }
                    else {
                        //添加到卡组
                        playerItemNow.cardDeck.PutOneCard(card);
                        cardDbProxy.RemoveOneCardEntry(card);
                        //该玩家的购买费用减少
                        playerItemNow.shipCard.cost -= card.cost;
                        SendNotification(UIViewSystemEvent.UI_CARD_DECK_LIST, playerItemNow, UIViewSystemEvent.UI_CARD_DECK_LIST_LOAD);
                        //下一回合
                        chooseStageCircuitProxy.IntoNextTurn();
                        List<CardEntry> cardEntries = cardDbProxy.GetSameCardEntry(3);
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, cardEntries, UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY);
                    }
                    
                    break;
               
            }
        }
        
    }
}
