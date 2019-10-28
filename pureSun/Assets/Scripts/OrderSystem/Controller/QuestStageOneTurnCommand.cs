

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class QuestStageOneTurnCommand : SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            QuestStageCircuitProxy circuitProxy = 
                Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            EffectInfoProxy effectInfoProxy =
                Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            OperateSystemProxy operateSystemProxy =
                Facade.RetrieveProxy(OperateSystemProxy.NAME) as OperateSystemProxy;
            //获取当前回合玩家
            PlayerGroupProxy playerGroupProxy = 
                Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            MinionGridProxy minionGridProxy =
             Facade.RetrieveProxy(MinionGridProxy.NAME) as MinionGridProxy;

            PlayerItem playerItemNow = playerGroupProxy.getPlayerByPlayerCode(circuitProxy.GetNowPlayerCode());
            switch (notification.Type)
            {
                //开始一个回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN:
                    UtilityLog.Log("一个回合开始");
                    circuitProxy.circuitItem.oneTurnStage = circuitProxy.circuitItem.questOneTurnStageList[0];
                    //开始一个阶段
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    break;
                //从指定阶段开始回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_ASSIGN_START_OF_TRUN:
                    circuitProxy.circuitItem.oneTurnStage = "ActivePhase";
                    UtilityLog.Log("一个回合从指定阶段开始");
                    //开始一个阶段
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    break;
                //结束一个回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_TRUN:
                    circuitProxy.IntoNextTurn();
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN);
                    break;
                //开始一个阶段
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE:
                    circuitProxy.circuitItem.oneStageStartAction();
                    circuitProxy.circuitItem.autoNextStage = true;
                    UtilityLog.Log("阶段开始：" + circuitProxy.circuitItem.oneTurnStage);
                    switch (circuitProxy.circuitItem.oneTurnStage)
                    {
                        //开始阶段
                        case "StartOfTrun":
                            //使用资源牌恢复到最大次数
                            playerItemNow.RestoreCanUseResourceNumMax();
                            //费用恢复至上限
                            playerItemNow.RestoreToTheUpperLimit();
                            //回合开始抽一张牌
                            playerItemNow.DrawCard(1);
                            break;
                        //主动阶段
                        case "ActivePhase":
                            circuitProxy.circuitItem.autoNextStage = false;
                            //判断当前玩家的种类
                            //AI玩家
                            if (playerItemNow.playerType == PlayerType.AIPlayer)
                            {
                                //切入到AI逻辑操作
                                SendNotification(LogicalSysEvent.LOGICAL_SYS, null, LogicalSysEvent.LOGICAL_SYS_ACTIVE_PHASE_ACTION);
                            }
                            //人类玩家
                            else if (playerItemNow.playerType == PlayerType.HumanPlayer) {
                                //显示下一回合按钮
                                SendNotification(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS_SHOW, playerItemNow.playerCode));
                            }
                            //直接返回
                            return;
                            break;
                        //守备阶段
                        case "OpponentDefense":
                            break;
                        //结算阶段
                        case "DelayedExecution":
                            break;
                        //战斗阶段
                        case "StartOfBattle":
                            //获取当前攻击方生物
                            List<MinionCellItem> minionCellItems = minionGridProxy.GetMinionCellItemListByPlayerCode(playerItemNow);

                            break;

                    }
                    UtilityLog.Log("是否可以自动结束：" + circuitProxy.circuitItem.autoNextStage);
                    if (circuitProxy.circuitItem.autoNextStage) {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE);
                       
                    }
                    break;
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE:
                    circuitProxy.circuitItem.oneStageEndAction();
                    int nowStageIndex = circuitProxy.circuitItem.questOneTurnStageList.IndexOf(circuitProxy.circuitItem.oneTurnStage);
                    if (nowStageIndex < circuitProxy.circuitItem.questOneTurnStageList.Count - 1)
                    {
                        circuitProxy.circuitItem.oneTurnStage = circuitProxy.circuitItem.questOneTurnStageList[nowStageIndex + 1];
                        UtilityLog.Log("进入下一个阶段");
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    }
                    else
                    {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_TRUN);
                    }
                    break;
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE:
                    UtilityLog.Log("检查是否可以结束阶段：" + circuitProxy.circuitItem.oneTurnStage);
                    bool canEndStage = true;
                    switch (circuitProxy.circuitItem.oneTurnStage)
                    {
                        //开始阶段
                        case "StartOfTrun":
                            
                            break;
                        //主动阶段
                        case "ActivePhase":
                            canEndStage = false;
                            //判断当前玩家的种类
                            //AI玩家
                            if (playerItemNow.playerType == PlayerType.AIPlayer)
                            {
                                //切入到AI逻辑操作
                                SendNotification(LogicalSysEvent.LOGICAL_SYS, null, LogicalSysEvent.LOGICAL_SYS_ACTIVE_PHASE_ACTION);
                            }
                            //人类玩家
                            else if (playerItemNow.playerType == PlayerType.HumanPlayer)
                            {
                                //人类玩家自行操作
                               
                            }
                            break;
                        //守备阶段
                        case "OpponentDefense":
                            break;
                        //结算阶段
                        case "DelayedExecution":
                            break;
                        //战斗阶段
                        case "StartOfBattle":
 
                            break;

                    }
                    if (canEndStage) {
                        //检查操作控制器和效果控制器是否都处于初始状态
                        if (operateSystemProxy.operateSystemItem.operateModeType == OperateSystemItem.OperateType.Close)
                        {
                            if (effectInfoProxy.effectSysItem.effectSysItemStage == EffectSysItemStage.UnStart)
                            {
                                UtilityLog.Log("自动结束阶段：" + circuitProxy.circuitItem.oneTurnStage);
                                SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE);
                            }
                        }   
                    }
                    
                   
                    break;
            }
        }
    }
}
