

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class QuestStageOneTurnCommand : SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            QuestStageCircuitProxy circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            switch (notification.Type)
            {
                //开始一个回合
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_START_OF_TRUN:
                    circuitProxy.circuitItem.oneTurnStage = circuitProxy.circuitItem.questOneTurnStageList[0];
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
                    bool autoNextStage = true;
                    //获取当前回合玩家
                    PlayerGroupProxy playerGroupProxy = Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
                    PlayerItem playerItem = playerGroupProxy.getPlayerByPlayerCode(circuitProxy.GetNowPlayerCode());
                    switch (circuitProxy.circuitItem.oneTurnStage)
                    {
                        //开始阶段
                        case QuestOneTurnStage.StartOfTrun:
                            //使用资源牌恢复到最大次数
                            playerItem.RestoreCanUseResourceNumMax();
                            //费用恢复至上限
                            playerItem.RestoreToTheUpperLimit();
                            //回合开始抽一张牌
                            playerItem.DrawCard(1);
                            break;
                        //主动阶段
                        case QuestOneTurnStage.ActivePhase:
                            autoNextStage = false;
                            //判断当前玩家的种类
                            //AI玩家
                            if (playerItem.playerType == PlayerType.AIPlayer)
                            {
                                //结束回合
                                SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE);
                            }
                            //人类玩家
                            else if (playerItem.playerType == PlayerType.HumanPlayer) {
                                //显示下一回合按钮
                                SendNotification(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS, null, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS_SHOW, playerItem.playerCode));
                            }
                            break;
                        //守备阶段
                        case QuestOneTurnStage.OpponentDefense:
                            break;
                        //结算阶段
                        case QuestOneTurnStage.DelayedExecution:
                            break;
                        //战斗阶段
                        case QuestOneTurnStage.StartOfBattle:
                            break;

                    }
                    if (autoNextStage) {
                        SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE);
                    }
                    break;
                case UIViewSystemEvent.UI_QUEST_TURN_STAGE_END_OF_STAGE:
                    circuitProxy.circuitItem.oneStageEndAction();
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
            }
        }
    }
}
