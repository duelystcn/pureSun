

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
                    circuitProxy.circuitItem.turnNum++;
                    UtilityLog.Log("玩家" + playerItemNow.playerCode +"第" + circuitProxy.circuitItem.turnNum + "回合开始" , LogColor.YELLOW);
                    circuitProxy.circuitItem.oneTurnStage = circuitProxy.circuitItem.questOneTurnStageList[0];
                    //开始一个阶段
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    break;
                //从指定阶段开始回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_ASSIGN_START_OF_TRUN:
                    GM_OneStageSite oneStageSite = circuitProxy.circuitItem.getOneStageSiteByStageCode("ActivePhase");
                    if (oneStageSite != null)
                    {
                        circuitProxy.circuitItem.oneTurnStage = oneStageSite;
                        circuitProxy.circuitItem.turnNum++;
                        UtilityLog.Log("一个回合从指定阶段开始");
                    }
                    else {
                        UtilityLog.LogError("找不到指定阶段的配置信息");
                    }
                   
                    //开始一个阶段
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    break;
                //结束一个回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_TRUN:
                    UtilityLog.Log("玩家" + playerItemNow.playerCode + "第" + circuitProxy.circuitItem.turnNum + "回合结束" , LogColor.YELLOW);
                    circuitProxy.IntoNextTurn();
                    SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN);
                    break;
                //开始一个阶段
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE:
                    circuitProxy.circuitItem.autoNextStage = true;
                    circuitProxy.circuitItem.questTurnStageState = QuestTurnStageState.StartOfState;
                    UtilityLog.Log("第" + circuitProxy.circuitItem.turnNum + "回合" + "开始一个阶段：" + circuitProxy.circuitItem.oneTurnStage.name, LogColor.YELLOW);
                    circuitProxy.circuitItem.oneStageStartAction(playerItemNow);
                    //List<MinionCellItem> minionCellItems = minionGridProxy.GetMinionCellItemListByPlayerCode(playerItemNow);
                    if (circuitProxy.circuitItem.autoNextStage) {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE);
                       
                    }
                    break;
                //执行一个阶段
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_EXECUTION_OF_STAGE:
                    circuitProxy.circuitItem.autoNextStage = true;
                    circuitProxy.circuitItem.questTurnStageState = QuestTurnStageState.ExecutionOfState;
                    UtilityLog.Log("第" + circuitProxy.circuitItem.turnNum + "回合" + "执行一个阶段：" + circuitProxy.circuitItem.oneTurnStage.name, LogColor.YELLOW);
                    circuitProxy.circuitItem.oneStageExecutionAction(playerItemNow);
                    if (circuitProxy.circuitItem.oneTurnStage.automatic == "N")
                    {
                        circuitProxy.circuitItem.autoNextStage = false;
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
                            //显示下一回合按钮
                            SendNotification(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS_SHOW, playerItemNow.playerCode));
                        }
                        //直接返回
                        return;
                    }
                    if (circuitProxy.circuitItem.autoNextStage)
                    {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE);

                    }
                    break;
                //结束一个阶段
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE:
                    circuitProxy.circuitItem.questTurnStageState = QuestTurnStageState.EndOfState;
                    UtilityLog.Log("第" + circuitProxy.circuitItem.turnNum + "回合" + "结束一个阶段：" + circuitProxy.circuitItem.oneTurnStage.name, LogColor.YELLOW);
                    circuitProxy.circuitItem.oneStageEndAction(playerItemNow);
                    int nowStageIndex = circuitProxy.circuitItem.questOneTurnStageList.IndexOf(circuitProxy.circuitItem.oneTurnStage);
                    if (nowStageIndex < circuitProxy.circuitItem.questOneTurnStageList.Count - 1)
                    {
                        circuitProxy.circuitItem.oneTurnStage = circuitProxy.circuitItem.questOneTurnStageList[nowStageIndex + 1];
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_STAGE);
                    }
                    else
                    {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_TRUN);
                    }
                    break;
                //检查当前阶段是否可以结束
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE:
                    UtilityLog.Log("第" + circuitProxy.circuitItem.turnNum + "回合" + "检查" + circuitProxy.circuitItem.oneTurnStage.name + "&&状态：" + circuitProxy.circuitItem.questTurnStageState.ToString() + "是否可以进入下一个状态", LogColor.YELLOW );
                    bool nextStageState = true;
                    if (circuitProxy.circuitItem.questTurnStageState == QuestTurnStageState.ExecutionOfState) {
                        if (circuitProxy.circuitItem.oneTurnStage.automatic == "N")
                        {
                            nextStageState = false;
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
                                    if (circuitProxy.circuitItem.questTurnStageState == QuestTurnStageState.StartOfState)
                                    {
                                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_EXECUTION_OF_STAGE);
                                    }
                                    else if (circuitProxy.circuitItem.questTurnStageState == QuestTurnStageState.ExecutionOfState)
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
