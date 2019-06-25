using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Circuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Hand;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class StartCircuitCommand: SimpleCommand
    {
        //开始流程
        public override void Execute(INotification notification)
        {
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;

            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;

            //玩家信息初始化
            playerGroupProxy.playerGroup.AddPlayer("TEST1");
            playerGroupProxy.playerGroup.AddPlayer("TEST2");
            foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
            {
                //创建牌库
                playerItem.cardDeck = new CardDeck();
                List<CardEntry> cardEntryList = new List<CardEntry>();
                for (int i = 0; i < 20; i++)
                {
                    CardEntry cardEntry = new CardEntry();
                    if (i % 3 == 0)
                    {
                        //生物
                        cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("TEST1"));   
                    }
                    else if (i % 3 == 1)
                    {
                        //事件
                        cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("TEST2"));
                    }
                    else
                    {
                        //资源
                        cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("TEST3"));
                    }
                    cardEntryList.Add(cardEntry);
                }
                playerItem.cardDeck.cardEntryList = cardEntryList;
                //增加手牌
                playerItem.handGridItem.CreateCell(playerItem.cardDeck.GetFirstCard());
                playerItem.handGridItem.CreateCell(playerItem.cardDeck.GetFirstCard());
                playerItem.handGridItem.CreateCell(playerItem.cardDeck.GetFirstCard());
            }

            


            CircuitProxy circuitProxy = Facade.RetrieveProxy(CircuitProxy.NAME) as CircuitProxy;
            circuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);
            //设置虚拟坐标
            playerGroupProxy.playerGroup.playerItems["TEST1"].hexCoordinates = new HexCoordinates(0,-1);
            playerGroupProxy.playerGroup.playerItems["TEST2"].hexCoordinates = new HexCoordinates(0, 4);

            UtilityLog.Log("进程初始化完毕");
            //手牌区渲染为当前回合玩家
            HandGridProxy handGridProxy = Facade.RetrieveProxy(HandGridProxy.NAME) as HandGridProxy;
            PlayerItem playerItemNow = playerGroupProxy.getPlayerByPlayerCode(circuitProxy.GetNowPlayerCode());
            handGridProxy.NowPlayerHandAfflux(playerItemNow.handGridItem);
            

        }
    }
}
