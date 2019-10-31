

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using Assets.Scripts.OrderSystem.Model.Database.Persistence;
using Assets.Scripts.OrderSystem.Model.Database.TestCase;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.Util;
using Assets.Scripts.OrderSystem.View.UIView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    public class QuestStageCommand : SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            EffectInfoProxy effectInfoProxy = Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            CardDbProxy cardDbProxy = Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            HexGridProxy hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            OperateSystemProxy operateSystemProxy = Facade.RetrieveProxy(OperateSystemProxy.NAME) as OperateSystemProxy;
            MinionGridProxy minionGridProxy =
               Facade.RetrieveProxy(MinionGridProxy.NAME) as MinionGridProxy;
            GameModelProxy gameModelProxy = Facade.RetrieveProxy(GameModelProxy.NAME) as GameModelProxy;

            QuestStageCircuitProxy questStageCircuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;

            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_QUEST_STAGE_START_SPECIAL:
                    TestCaseInfo chooseOneTestCase = notification.Body as TestCaseInfo;
                    //读取默认设置
                    GameInterfacePreparationByGameModel(gameModelProxy, 
                                                        playerGroupProxy, 
                                                        questStageCircuitProxy,
                                                        effectInfoProxy,
                                                        chooseOneTestCase.gameModelName);

                    //读取存储json文件
                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        PI_Player pI_Player = new PI_Player();
                        if (playerItem.playerCode == "TEST1")
                        {
                            pI_Player = chooseOneTestCase.myselfPlayer;
                        }
                        else
                        {
                            pI_Player = chooseOneTestCase.enemyPlayer;
                        }
                        //创建牌库
                        playerItem.cardDeck = new CardDeck();
                        foreach (string cardName in pI_Player.deckCard)
                        {
                            CardEntry cardEntry = new CardEntry();
                            cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode(cardName));
                            cardEntry.player = playerItem;
                            playerItem.cardDeck.cardEntryList.Add(cardEntry);
                        }
                        CardEntry shipCard = new CardEntry();
                        shipCard.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode(pI_Player.shipCardCode));
                        playerItem.shipCard = shipCard;
                        //手牌
                        foreach (string cardName in pI_Player.handCard)
                        {
                            CardEntry cardEntry = new CardEntry();
                            cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode(cardName));
                            cardEntry.player = playerItem;
                            playerItem.AddCardToHandAndNoTT(cardEntry);

                        }
                        //费用上限和可用费用
                        playerItem.manaItem.manaUpperLimit = pI_Player.manaUpperLimit;
                        playerItem.manaItem.manaUsable = pI_Player.manaUpperLimit;
                        //科技
                        foreach (string trait in pI_Player.traitList)
                        {
                            playerItem.traitCombination.AddTraitType(trait);
                        }
                        //刷新手牌是否可用
                        playerItem.ChangeHandCardCanUse();

                        //生物渲染？
                        foreach (PI_Minion pI_Minion in pI_Player.minionList) {
                            HexCoordinates hexCoordinates = new HexCoordinates(pI_Minion.x, pI_Minion.z);
                            int index = HexUtil.GetIndexFromModeAndHex(gameModelProxy.hexModelInfoNow, hexCoordinates);
                            CardEntry cardEntry = new CardEntry();
                            cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode(pI_Minion.code));
                            cardEntry.player = playerItem;
                            minionGridProxy.AddOneMinionByCard(index, cardEntry);
                            UtilityLog.Log("读取一个生物：" + cardEntry.cardInfo.name);

                        }
                      
                    }
                    questStageCircuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);
                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {

                        //先开启手牌栏
                        SendNotification(HandSystemEvent.HAND_VIEW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_VIEW_SYS_INIT_PLAYER_CODE, playerItem.playerCode));
                        //手牌渲染
                        SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_AFFLUX, playerItem.playerCode));
                        //获取船只的效果，如果是持续效果则添加到全局监听中
                        foreach (string effectCode in playerItem.shipCard.effectCodeList)
                        {
                            EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectCode);
                            if (oneEffectInfo.impactType == "Continue")
                            {
                                oneEffectInfo.player = playerItem;
                                oneEffectInfo.cardEntry = playerItem.shipCard;
                                questStageCircuitProxy.circuitItem.putOneEffectInfoInActiveMap(oneEffectInfo, effectInfoProxy.effectSysItem.impactTimeTriggerMap);
                            }

                        }

                        ManaItem manaItem = new ManaItem();
                        manaItem.manaUpperLimit = playerItem.manaItem.manaUpperLimit;
                        manaItem.manaUsable = playerItem.manaItem.manaUsable;

                        SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, manaItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_INIT, playerItem.playerCode));
                        SendNotification(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS, playerItem.traitCombination.traitTypes, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_INIT, playerItem.playerCode));
                    }
                    SendNotification(
                             UIViewSystemEvent.UI_VIEW_CURRENT,
                             null,
                             StringUtil.GetNTByNotificationTypeAndUIViewName(
                                 UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                 UIViewConfig.getNameStrByUIViewName(UIViewName.NextTurnButton)
                                 )
                             );
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_ASSIGN_START_OF_TRUN);

                    break;
                case UIViewSystemEvent.UI_QUEST_STAGE_START:
                    //读取默认设置
                    GameInterfacePreparationByGameModel(gameModelProxy, 
                                                        playerGroupProxy, 
                                                        questStageCircuitProxy,
                                                        effectInfoProxy,
                                                        "Andor");

                    questStageCircuitProxy.CircuitStart(playerGroupProxy.playerGroup.playerItems);
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
                                cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("ImperialRecruit"));
                            }
                            else if (i % 3 == 1)
                            {
                                //事件
                                cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("FortifiedAgent"));
                            }
                            else
                            {
                                //资源
                                cardEntry.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("TaxCar"));
                            }
                            cardEntry.player = playerItem;
                            cardEntryList.Add(cardEntry);
                        }
                        playerItem.cardDeck.cardEntryList = cardEntryList;
                        CardEntry shipCard = new CardEntry();
                        shipCard.InitializeByCardInfo(cardDbProxy.GetCardInfoByCode("FindWay"));
                        playerItem.shipCard = shipCard;
                    }
                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        //先开启手牌栏
                        SendNotification(HandSystemEvent.HAND_VIEW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_VIEW_SYS_INIT_PLAYER_CODE, playerItem.playerCode));

                        //分发手牌
                        playerItem.DrawCard(3);
                        //设置起始费用上限
                        playerItem.manaItem.manaUpperLimit = 1;
                        //设置当前费用为0
                        playerItem.manaItem.manaUsable = 0;
                        //获取船只的效果，如果是持续效果则添加到全局监听中
                        foreach (string effectCode in playerItem.shipCard.effectCodeList)
                        {
                            EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectCode);
                            if (oneEffectInfo.impactType == "Continue") {
                                oneEffectInfo.player = playerItem;
                                oneEffectInfo.cardEntry = playerItem.shipCard;
                                questStageCircuitProxy.circuitItem.putOneEffectInfoInActiveMap(oneEffectInfo, effectInfoProxy.effectSysItem.impactTimeTriggerMap);
                            }

                        }
                        ManaItem manaItem = new ManaItem();
                        manaItem.manaUpperLimit = playerItem.manaItem.manaUpperLimit;
                        manaItem.manaUsable = playerItem.manaItem.manaUsable;
                        SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, manaItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_INIT,playerItem.playerCode));

                        SendNotification(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS, playerItem.traitCombination.traitTypes, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_INIT, playerItem.playerCode));
                        //手牌渲染
                        //SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem, StringUtil.NotificationTypeAddPlayerCode(HandSystemEvent.HAND_CHANGE_AFFLUX, playerItem.playerCode));
                    }
                    SendNotification(
                            UIViewSystemEvent.UI_VIEW_CURRENT,
                            null,
                            StringUtil.GetNTByNotificationTypeAndUIViewName(
                                UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                UIViewConfig.getNameStrByUIViewName(UIViewName.NextTurnButton)
                                )
                            );
                    UtilityLog.Log("测试准备完成" );
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN);
                    break;
            }
        }
        //读取模式并做出一定的设置
        public void GameInterfacePreparationByGameModel(GameModelProxy gameModelProxy,
                                                        PlayerGroupProxy playerGroupProxy,
                                                        QuestStageCircuitProxy questStageCircuitProxy,
                                                        EffectInfoProxy effectInfoProxy,
                                                        string gameModelName)
        {
            gameModelProxy.setGameModelNow(gameModelName);

            HexGridProxy hexGridProxy = Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            hexGridProxy.InitializeTheProxy(gameModelProxy.hexModelInfoNow);

            int number = 0;
            foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
            {
                GM_PlayerSite playerSiteOne = gameModelProxy.gameModelNow.playerSiteList[number];
                playerItem.CreateCanCallHex(playerSiteOne);
                number++;
            }
            //初始化流程信息
            //将流程需要的监听放入到监听效果集合中
            for (int n = 0; n < gameModelProxy.gameModelNow.turnStage.Length; n++)
            {
                GM_OneStageSite oneStageSite = gameModelProxy.stageSiteMap[gameModelProxy.gameModelNow.turnStage[n]];
                questStageCircuitProxy.circuitItem.questOneTurnStageList.Add(oneStageSite);
                foreach (string effectCode in oneStageSite.effectNeedExeList) {
                    //创建空的实体信息，在执行的时候会用到
                    EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectCode);
                    CardEntry cardEntry = new CardEntry();
                    PlayerItem player = new PlayerItem("NONE");
                    oneEffectInfo.cardEntry = cardEntry;
                    oneEffectInfo.player = player;

                    if (oneEffectInfo.impactType == "GameModelRule")
                    {
                        questStageCircuitProxy.circuitItem.putOneEffectInfoInActiveMap(oneEffectInfo, effectInfoProxy.effectSysItem.impactTimeTriggerMap);
                    }
                }
            }

            //通知渲染战场
            SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_SYS_SHOW_START);
            //渲染费用栏
            SendNotification(
                                UIViewSystemEvent.UI_VIEW_CURRENT,
                                null,
                                StringUtil.GetNTByNotificationTypeAndUIViewName(
                                    UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                    UIViewConfig.getNameStrByUIViewName(UIViewName.ManaInfoView)
                                    )
                                );
            //渲染科技栏
            SendNotification(
                              UIViewSystemEvent.UI_VIEW_CURRENT,
                              null,
                              StringUtil.GetNTByNotificationTypeAndUIViewName(
                                  UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                  UIViewConfig.getNameStrByUIViewName(UIViewName.TraitCombinationView)
                                  )
                              );
            //渲染船只栏
            SendNotification(
                              UIViewSystemEvent.UI_VIEW_CURRENT,
                              null,
                              StringUtil.GetNTByNotificationTypeAndUIViewName(
                                  UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                  UIViewConfig.getNameStrByUIViewName(UIViewName.ShipComponentView)
                                  )
                              );
        }
    }
}
