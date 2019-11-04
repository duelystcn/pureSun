

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.OrderSystem.Model.Database.Card.CardEntry;

namespace Assets.Scripts.OrderSystem.Controller
{
    //我希望把所有需要判断操作的都放到这里，来判断玩家操作和AI操作
    public class LogicalProcessorCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            OperateSystemProxy operateSystemProxy =
               Facade.RetrieveProxy(OperateSystemProxy.NAME) as OperateSystemProxy;
            QuestStageCircuitProxy circuitProxy =
              Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            PlayerGroupProxy playerGroupProxy = 
                Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            ChooseStageCircuitProxy chooseStageCircuitProxy = 
                Facade.RetrieveProxy(ChooseStageCircuitProxy.NAME) as ChooseStageCircuitProxy;
            EffectInfoProxy effectInfoProxy =
               Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            HexGridProxy hexGridProxy =
                      Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;
            PlayerItem playerItem = null;
            switch (notification.Type)
            {
                //选择船只逻辑处理
                case LogicalSysEvent.LOGICAL_SYS_CHOOSE_SHIP_CARD:
                    string playerCode = notification.Body as string;
                    playerItem = playerGroupProxy.getPlayerByPlayerCode(playerCode);
                    //如果是玩家
                    if (playerItem.playerType == PlayerType.HumanPlayer)
                    {
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE,
                                        chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode],
                                        StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY, playerCode));

                    }
                    //AI玩家
                    else if (playerItem.playerType == PlayerType.AIPlayer)
                    {
                        //AI进行船只渲染
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode], StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY, playerCode));
                        //AI选择船只
                        CardEntry shipCardEntry = chooseStageCircuitProxy.chooseStageCircuitItem.playerShipCardMap[playerCode][0];
                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, shipCardEntry, UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD);
                    }
                    //网络用户
                    else if (playerItem.playerType == PlayerType.NetPlayer)
                    {
                    }
                    break;
                case LogicalSysEvent.LOGICAL_SYS_CHOOSE_EFFECT:
                    EffectInfo effectInfo = notification.Body as EffectInfo;
                    playerItem = effectInfo.chooseByPlayer;
                    //如果是玩家
                    if (playerItem.playerType == PlayerType.HumanPlayer)
                    {
                        //整理好效果内容，弹出选择框让玩家选择
                        //把这些效果实例化成卡片
                        List<CardEntry> cardEntries = new List<CardEntry>();
                        foreach (string effectName in effectInfo.chooseEffectList) {
                            EffectInfo oneEffectInfo = effectInfoProxy.effectSysItem.effectInfoMap[effectName];
                            CardEntry oneCardEntry = new CardEntry();
                            oneCardEntry.InitializeByCardInfo(effectInfo.cardEntry.cardInfo);
                            oneCardEntry.InitializeByEffectInfo(oneEffectInfo);
                            cardEntries.Add(oneCardEntry);
                        }
                        SendNotification(UIViewSystemEvent.UI_USER_OPERAT, cardEntries,
                                          StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_USER_OPERAT_CHOOSE_EFFECT, playerItem.playerCode));


                    }
                    //AI玩家
                    else if (playerItem.playerType == PlayerType.AIPlayer)
                    {
                        UtilityLog.Log("AI玩家【" + playerItem.playerCode + "】开始选择卡牌效果",LogUtType.Operate);
                        //先直接选择第一种
                        EffectInfo oneEffectInfo = effectInfoProxy.effectSysItem.effectInfoMap[effectInfo.chooseEffectList[0]];
                        CardEntry oneCardEntry = new CardEntry();
                        oneCardEntry.InitializeByCardInfo(effectInfo.cardEntry.cardInfo);
                        oneCardEntry.InitializeByEffectInfo(oneEffectInfo);
                        SendNotification(OperateSystemEvent.OPERATE_SYS, oneCardEntry, OperateSystemEvent.OPERATE_SYS_CHOOSE_ONE_EFFECT);
                    }
                    //网络用户
                    else if (playerItem.playerType == PlayerType.NetPlayer)
                    {
                    }
                    break;
                case LogicalSysEvent.LOGICAL_SYS_ACTIVE_PHASE_ACTION:
                    //获取当前进行游戏的玩家进行接管
                    string playerCodeNow = circuitProxy.GetNowPlayerCode();
                    PlayerItem playerItemNow = playerGroupProxy.getPlayerByPlayerCode(playerCodeNow);
                    UtilityLog.Log("AI玩家" + playerCodeNow + "开始操作：", LogUtType.Operate);
                    //无法操作了结束回合
                    bool canContinueOperation = true;

                    //判断是否使用过资源牌
                    if (playerItemNow.CheckResourceCardCanUse())
                    {
                        UtilityLog.Log("AI玩家" + playerCodeNow + "可以使用资源牌：", LogUtType.Operate);
                        HandCellItem getHand = playerItemNow.GetOneCardTypeCard(CardType.ResourceCard);
                        //检查手牌里是否存在资源牌
                        if (getHand != null)
                        {
                            //使用这张手牌
                            operateSystemProxy.IntoModeHandUse(getHand,playerItemNow);
                            HexCellItem hexCellItem = new HexCellItem(1,1);
                            SendNotification(OperateSystemEvent.OPERATE_SYS, hexCellItem, OperateSystemEvent.OPERATE_SYS_DRAW_END_HEX);
                            
                        }
                        else {

                            canContinueOperation = false;
                        }
                    }
                    else {
                        UtilityLog.Log("AI玩家" + playerCodeNow + "不可以使用资源牌：", LogUtType.Operate);
                        canContinueOperation = false;
                    }

                    if (!canContinueOperation)
                    {
                        //结束回合
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE);
                    }
                    else {
                        circuitProxy.circuitItem.autoNextStage = false;
                    }
                    break;
            }
        }
       
    }
   
}
