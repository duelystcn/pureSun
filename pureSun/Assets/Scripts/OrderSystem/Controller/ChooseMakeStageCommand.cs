

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using static Assets.Scripts.OrderSystem.Model.Database.Card.CardEntry;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class ChooseMakeStageCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            ChooseStageCircuitProxy chooseStageCircuitProxy = Facade.RetrieveProxy(ChooseStageCircuitProxy.NAME) as ChooseStageCircuitProxy;
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_ONE_CARD:
                    CardEntry card = notification.Body as CardEntry;
                    string playerCodeChoose = chooseStageCircuitProxy.GetNowPlayerCode();
                    PlayerItem playerItemNow = playerGroupProxy.getPlayerByPlayerCode(playerCodeChoose);
                    //如果能成功购买，该玩家的购买费用减少，否则返回购买力不够
                    if (playerItemNow.shipCard.cost >= card.cost)
                    {
                        //添加到卡组
                        playerItemNow.cardDeck.PutOneCard(card);
                        // cardDbProxy.RemoveOneCardEntry(card);
                        playerItemNow.shipCard.cost -= card.cost;
                        SendNotification(UIViewSystemEvent.UI_CARD_DECK_LIST, playerItemNow, UIViewSystemEvent.UI_CARD_DECK_LIST_LOAD);
                        card.isBuyed = true;
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE, card, UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_ONE_CARD_SUCCESS);

                        //判断是否大于6回合
                        if (chooseStageCircuitProxy.chooseStageCircuitItem.chooseIndex == 7)
                        {
                            SendNotification(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE, cardDbProxy.GetOneCardListForPool(), UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_CLOSE);
                            SendNotification(UIViewSystemEvent.UI_QUEST_STAGE, null, UIViewSystemEvent.UI_QUEST_STAGE_START);
                        }
                        else
                        {
                            //下一回合
                            chooseStageCircuitProxy.IntoNextTurn();
                            SendNotification(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE, cardDbProxy.GetOneCardListForPool(), UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_LOAD_NEXT_LIST);
                        }
                    }
                    else {
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE, card, UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_ONE_CARD_FAILURE);
                    }
                 
                    break;
            }
        }
    }
}
