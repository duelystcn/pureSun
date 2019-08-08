﻿

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Patterns.Proxy;

namespace Assets.Scripts.OrderSystem.Model.Player
{
    public class PlayerGroupProxy : Proxy
    {
        public new const string NAME = "PlayerGroupProxy";
        
        public PlayerGroup playerGroup
        {
            get { return (PlayerGroup)base.Data; }
        }
        public PlayerGroupProxy() : base(NAME)
        {
            PlayerGroup playerGroup = new PlayerGroup();
            playerGroup.Create();
            base.Data = playerGroup;
           
        }
        //增加玩家
        public void AddPlayer(string playerCode,PlayerType playerType) {
            PlayerItem playerItem = null;
            if (playerType == PlayerType.HumanPlayer)
            {
                playerItem = playerGroup.AddHumanPlayer(playerCode);
            }
            else if (playerType == PlayerType.AIPlayer) {
                playerItem = playerGroup.AddAIPlayer(playerCode);
            }
            if (playerItem == null)
            {
                UtilityLog.LogError("玩家创建失败");
            }
            else {
                AddTimeTrigger(playerItem);
            }

        }

        //绑定时点触发器
        public void AddTimeTrigger(PlayerItem playerItem)
        {
            playerItem.ttPlayerDrawACard = (HandCellItem handCellItem) =>
            {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, handCellItem, TimeTriggerEvent.TIME_TRIGGER_DRAW_A_CARD);
                SendNotification(HandSystemEvent.HAND_CHANGE, handCellItem, StringUtil.NotificationTypeAddPlayerCode(HandSystemEvent.HAND_CHANGE_DRAW_ONE_CARD,playerItem.playerCode));
            };
            playerItem.ttPlayerRemoveACard = (HandCellItem handCellItem) =>
            {
              
                SendNotification(HandSystemEvent.HAND_CHANGE, handCellItem, StringUtil.NotificationTypeAddPlayerCode(HandSystemEvent.HAND_CHANGE_REMOVE_ONE_CARD, playerItem.playerCode));
            };



        }

        //获取指定玩家的信息
        public PlayerItem getPlayerByPlayerCode(string playerCode) {
            PlayerItem returnItem = playerGroup.playerItems[playerCode];
            if (returnItem == null) {
                UtilityLog.LogError("玩家信息为空");
            }
            return returnItem;
        }
        //查看是否所有玩家都选择了船
        public bool checkAllPlayerHasShip() {
            bool allHas = true;
            foreach (PlayerItem playerItem in playerGroup.playerItems.Values) {
                if (playerItem.shipCard == null) {
                    allHas = false;
                }
            }
            return allHas;
        }
        

    }
}
