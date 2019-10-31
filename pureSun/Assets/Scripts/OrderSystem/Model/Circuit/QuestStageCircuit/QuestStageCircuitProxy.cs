using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit
{
    public class QuestStageCircuitProxy : Proxy
    {
        public new const string NAME = "QuestStageCircuitProxy";

        public QuestStageCircuitItem circuitItem
        {
            get { return (QuestStageCircuitItem)base.Data; }
        }
        public QuestStageCircuitProxy() : base(NAME) {
            QuestStageCircuitItem circuitItem = new QuestStageCircuitItem();
            base.Data = circuitItem;
            circuitItem.oneStageStartAction = (PlayerItem playerItem) =>
            {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, circuitItem.oneTurnStage.code, StringUtil.GetNTByNotificationTypeAndPlayerCode(TimeTriggerEvent.TIME_TRIGGER_SYS_ONE_STAGE_START, playerItem.playerCode));
            };
            circuitItem.oneStageEndAction = (PlayerItem playerItem) =>
            {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, circuitItem.oneTurnStage.code, StringUtil.GetNTByNotificationTypeAndPlayerCode(TimeTriggerEvent.TIME_TRIGGER_SYS_ONE_STAGE_END, playerItem.playerCode));
            };
            circuitItem.oneStageExecutionAction = (PlayerItem playerItem) =>
            {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, circuitItem.oneTurnStage.code, StringUtil.GetNTByNotificationTypeAndPlayerCode(TimeTriggerEvent.TIME_TRIGGER_SYS_ONE_STAGE_EXECUTION, playerItem.playerCode));
            };

        }


        //流程开始
        public void CircuitStart(Dictionary<string, PlayerItem> dictionary ) {
            CreatePlayerOrder(dictionary);
            circuitItem.nowPlayerCode = circuitItem.playerOrder[0];
        }

        

        //创建玩家流程序列
        public void CreatePlayerOrder(Dictionary<string, PlayerItem> dictionary) {
            circuitItem.playerOrder = new List<string>();
            foreach (string playerCode in dictionary.Keys) {
                circuitItem.playerOrder.Add(playerCode);
            }

        }
        //获取当前玩家
        public string GetNowPlayerCode() {
            return circuitItem.nowPlayerCode;
        }
        //进入下一个玩家的回合
        public void IntoNextTurn() {
            int idx = circuitItem.playerOrder.IndexOf(circuitItem.nowPlayerCode);
            if (idx == circuitItem.playerOrder.Count - 1)
            {
                circuitItem.nowPlayerCode = circuitItem.playerOrder[0];
            }
            else {
                circuitItem.nowPlayerCode = circuitItem.playerOrder[idx+1];
            }
        }

    }
}
