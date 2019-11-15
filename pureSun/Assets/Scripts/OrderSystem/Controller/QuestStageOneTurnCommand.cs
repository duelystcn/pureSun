

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
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
            QuestStageCircuitProxy questStageCircuitProxy = 
                Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            EffectInfoProxy effectInfoProxy =
                Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            OperateSystemProxy operateSystemProxy =
                Facade.RetrieveProxy(OperateSystemProxy.NAME) as OperateSystemProxy;
            //获取当前回合玩家
            PlayerGroupProxy playerGroupProxy = 
                Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;


           
            PlayerItem turnHavePlayerItem = playerGroupProxy.getPlayerByPlayerCode(questStageCircuitProxy.GetNowHaveTurnPlayerCode());
            switch (notification.Type)
            {
                //开始一个回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN:
                    questStageCircuitProxy.circuitItem.turnNum++;
                    UtilityLog.Log("玩家【" + turnHavePlayerItem.playerCode +"】第【" + questStageCircuitProxy.circuitItem.turnNum + "】回合开始" , LogUtType.Stage);
                    questStageCircuitProxy.circuitItem.oneTurnStage = questStageCircuitProxy.circuitItem.questOneTurnStageList[0];
                    //开始一个阶段
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    break;
                //从指定阶段开始回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_ASSIGN_START_OF_TRUN:
                    GM_OneStageSite oneStageSite = questStageCircuitProxy.circuitItem.getOneStageSiteByStageCode("ActivePhase");
                    if (oneStageSite != null)
                    {
                        questStageCircuitProxy.circuitItem.oneTurnStage = oneStageSite;
                        questStageCircuitProxy.circuitItem.turnNum++;
                        UtilityLog.Log("一个回合从指定阶段开始", LogUtType.Stage);
                    }
                    else {
                        UtilityLog.LogError("找不到指定阶段的配置信息");
                    }
                   
                    //开始一个阶段
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    break;
                //结束一个回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_TRUN:
                    UtilityLog.Log("玩家【" + turnHavePlayerItem.playerCode + "】第【" + questStageCircuitProxy.circuitItem.turnNum + "】回合结束" , LogUtType.Stage);
                    questStageCircuitProxy.IntoNextTurn();
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN);
                    break;
                //开始一个阶段
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE:
                    questStageCircuitProxy.circuitItem.autoNextStage = true;
                    if (questStageCircuitProxy.circuitItem.oneTurnStage.operatingPlayer == "Myself")
                    {
                        questStageCircuitProxy.SetNowHaveStagePlayerCode(questStageCircuitProxy.GetNowHaveTurnPlayerCode());
                    }
                    else if(questStageCircuitProxy.circuitItem.oneTurnStage.operatingPlayer == "Enemy") {
                        questStageCircuitProxy.SetNowHaveStagePlayerCode(playerGroupProxy.getEnemytPlayerByPlayerCode(questStageCircuitProxy.GetNowHaveTurnPlayerCode()).playerCode);
                    }
                    questStageCircuitProxy.circuitItem.questTurnStageState = QuestTurnStageState.StartOfState;
                    UtilityLog.Log("第【" + questStageCircuitProxy.circuitItem.turnNum + "】回合" + "开始一个阶段【" + questStageCircuitProxy.circuitItem.oneTurnStage.name + "】", LogUtType.Stage);
                    SendNotification(UIViewSystemEvent.UI_TURN_STAGE_SYS, questStageCircuitProxy.circuitItem, UIViewSystemEvent.UI_TURN_STAGE_SYS_STAGE_CHANGE);
                    effectInfoProxy.effectSysItem.showEffectNum++;
                    questStageCircuitProxy.circuitItem.oneStageStartAction(turnHavePlayerItem);
                    if (questStageCircuitProxy.circuitItem.autoNextStage) {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE);
                       
                    }
                    break;
                //执行一个阶段
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_EXECUTION_OF_STAGE:
                    PlayerItem stageHavePlayerItemForExe = playerGroupProxy.getPlayerByPlayerCode(questStageCircuitProxy.GetNowHaveStagePlayerCode());
                    stageHavePlayerItemForExe.ttPlayerHandCanUseJudge();
                    questStageCircuitProxy.circuitItem.autoNextStage = true;
                    questStageCircuitProxy.circuitItem.questTurnStageState = QuestTurnStageState.ExecutionOfState;
                    UtilityLog.Log("第【" + questStageCircuitProxy.circuitItem.turnNum + "】回合" + "执行一个阶段【" + questStageCircuitProxy.circuitItem.oneTurnStage.name + "】", LogUtType.Stage);
                    questStageCircuitProxy.circuitItem.oneStageExecutionAction(turnHavePlayerItem);
                    if (questStageCircuitProxy.circuitItem.oneTurnStage.automatic == "N")
                    {
                        questStageCircuitProxy.circuitItem.autoNextStage = false;
                        //判断当前玩家的种类
                        //AI玩家
                        if (stageHavePlayerItemForExe.playerType == PlayerType.AIPlayer)
                        {
                            //切入到AI逻辑操作
                            SendNotification(LogicalSysEvent.LOGICAL_SYS, null, LogicalSysEvent.LOGICAL_SYS_ACTIVE_PHASE_ACTION);
                        }
                        //人类玩家
                        else if (stageHavePlayerItemForExe.playerType == PlayerType.HumanPlayer)
                        {
                            //显示结束阶段按钮
                            SendNotification(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS_SHOW, stageHavePlayerItemForExe.playerCode));
                        }
                        //直接返回
                        return;
                    }
                    if (questStageCircuitProxy.circuitItem.autoNextStage)
                    {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE);

                    }
                    break;
                //结束一个阶段
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE:
                    questStageCircuitProxy.circuitItem.questTurnStageState = QuestTurnStageState.EndOfState;
                    UtilityLog.Log("第【" + questStageCircuitProxy.circuitItem.turnNum + "】回合" + "结束一个阶段【" + questStageCircuitProxy.circuitItem.oneTurnStage.name + "】", LogUtType.Stage);
                    questStageCircuitProxy.circuitItem.oneStageEndAction(turnHavePlayerItem);
                    int nowStageIndex = questStageCircuitProxy.circuitItem.questOneTurnStageList.IndexOf(questStageCircuitProxy.circuitItem.oneTurnStage);
                    if (nowStageIndex < questStageCircuitProxy.circuitItem.questOneTurnStageList.Count - 1)
                    {
                        questStageCircuitProxy.circuitItem.oneTurnStage = questStageCircuitProxy.circuitItem.questOneTurnStageList[nowStageIndex + 1];
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    }
                    else
                    {
                        questStageCircuitProxy.circuitItem.oneTurnEndAction(turnHavePlayerItem);
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_TRUN);
                    }
                    break;
                //检查当前阶段是否可以结束
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE:
                    PlayerItem stageHavePlayerItemForCheckEnd = playerGroupProxy.getPlayerByPlayerCode(questStageCircuitProxy.GetNowHaveStagePlayerCode());
                    UtilityLog.Log("第【" + questStageCircuitProxy.circuitItem.turnNum + "】回合" + "检查【" + questStageCircuitProxy.circuitItem.oneTurnStage.name + "】&&状态【" + questStageCircuitProxy.circuitItem.questTurnStageState.ToString() + "】是否可以进入下一个状态", LogUtType.Stage);
                    bool nextStageState = true;
                    if (questStageCircuitProxy.circuitItem.questTurnStageState == QuestTurnStageState.ExecutionOfState) {
                        if (questStageCircuitProxy.circuitItem.oneTurnStage.automatic == "N")
                        {
                            nextStageState = false;
                            //判断当前玩家的种类
                            //AI玩家
                            if (stageHavePlayerItemForCheckEnd.playerType == PlayerType.AIPlayer)
                            {
                                //切入到AI逻辑操作
                                SendNotification(LogicalSysEvent.LOGICAL_SYS, null, LogicalSysEvent.LOGICAL_SYS_ACTIVE_PHASE_ACTION);
                            }
                            //人类玩家
                            else if (stageHavePlayerItemForCheckEnd.playerType == PlayerType.HumanPlayer)
                            {
                                //人类玩家自行操作

                            }
                        }
                    }
                    if (nextStageState) {
                        //检查操作控制器和效果控制器是否都处于初始状态
                        if (operateSystemProxy.operateSystemItem.operateModeType == OperateSystemItem.OperateType.Close)
                        {
                            if (effectInfoProxy.effectSysItem.effectSysItemStage == EffectSysItemStage.UnStart)
                            {
                                //检查效果是否都展示完毕
                                if (effectInfoProxy.effectSysItem.showEffectNum == 0)
                                {
                                    if (questStageCircuitProxy.circuitItem.questTurnStageState == QuestTurnStageState.StartOfState)
                                    {
                                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_EXECUTION_OF_STAGE);
                                    }
                                    else if (questStageCircuitProxy.circuitItem.questTurnStageState == QuestTurnStageState.ExecutionOfState)
                                    {
                                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE);
                                    }
                                }
                            }
                        }   
                    }
                    
                   
                    break;
            }
        }
    }
}
