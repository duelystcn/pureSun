

using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit
{
    public class ChooseStageCircuitProxy : Proxy
    {
        public new const string NAME = "ChooseStageCircuitProxy";

        public ChooseStageCircuitItem chooseStageCircuitItem
        {
            get { return (ChooseStageCircuitItem)base.Data; }
        }
        public ChooseStageCircuitProxy() : base(NAME)
        {
            ChooseStageCircuitItem chooseStageCircuitItem = new ChooseStageCircuitItem();
            base.Data = chooseStageCircuitItem;
        }
        //流程开始
        public void CircuitStart(Dictionary<string, PlayerItem> dictionary)
        {
            CreatePlayerOrder(dictionary);
            chooseStageCircuitItem.nowPlayerCode = chooseStageCircuitItem.playerOrder[0];
        }
        //创建玩家流程序列
        public void CreatePlayerOrder(Dictionary<string, PlayerItem> dictionary)
        {
            chooseStageCircuitItem.playerOrder = new List<string>();
            foreach (string playerCode in dictionary.Keys)
            {
                List<CardInfo> cardInfos = new List<CardInfo>();
                chooseStageCircuitItem.playerShipCardMap.Add(playerCode, cardInfos);
                chooseStageCircuitItem.playerOrder.Add(playerCode);
            }

        }
        //获取当前玩家
        public string GetNowPlayerCode()
        {
            return chooseStageCircuitItem.nowPlayerCode;
        }
        //进入下一个玩家的回合
        public void IntoNextTurn()
        {
            int idx = chooseStageCircuitItem.playerOrder.IndexOf(chooseStageCircuitItem.nowPlayerCode);
            if (idx == chooseStageCircuitItem.playerOrder.Count - 1)
            {
                chooseStageCircuitItem.nowPlayerCode = chooseStageCircuitItem.playerOrder[0];
            }
            else
            {
                chooseStageCircuitItem.nowPlayerCode = chooseStageCircuitItem.playerOrder[idx + 1];
            }
        }

    }
}
