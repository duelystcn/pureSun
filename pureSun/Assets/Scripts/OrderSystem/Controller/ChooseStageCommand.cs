
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class ChooseStageCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_CHOOSE_STAGE_START:
                    ChooseStageStart();
                    break;
            }
        }
        private void ChooseStageStart() {
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            List<CardInfo> shipCardList = cardDbProxy.GetCardInfoByType(CardMetrics.CARD_TYPE_SHIP);
            UtilityLog.Log(shipCardList.Count);
        }
    }
}
