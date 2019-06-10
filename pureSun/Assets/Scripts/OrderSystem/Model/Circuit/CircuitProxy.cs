

using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Circuit
{
    public class CircuitProxy : Proxy
    {
        public new const string NAME = "CircuitProxy";

        public CircuitItem circuitItem
        {
            get { return (CircuitItem)base.Data; }
        }
        public CircuitProxy() : base(NAME) {
            CircuitItem circuitItem = new CircuitItem();
            base.Data = circuitItem;
        }
        //流程开始
        public void CircuitStart(Dictionary<string, PlayerItem> dictionary ) {
            circuitItem.turnNum = 1;
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
