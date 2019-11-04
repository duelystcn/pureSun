

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
            //抽一张牌
            playerItem.ttPlayerDrawACard = (HandCellItem handCellItem) =>
            {
                SendNotification(UIViewSystemEvent.UI_VIEW_ZF_HAND_CHANGE, handCellItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_DRAW_ONE_CARD,playerItem.playerCode));
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, handCellItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD, playerItem.playerCode));
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);

            };
            //判断手牌是否可用
            playerItem.ttPlayerHandCanUseJudge = () =>
            {
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);
            };
            //使用一张牌
            playerItem.ttPlayerUseACard = (HandCellItem handCellItem) =>
            {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, handCellItem, TimeTriggerEvent.TIME_TRIGGER_SYS_USE_HAND_CARD);
            };
            //移除一张牌
            playerItem.ttPlayerRemoveACard = (HandCellItem handCellItem) =>
            {
                SendNotification(HandSystemEvent.HAND_CHANGE, handCellItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_REMOVE_ONE_CARD, playerItem.playerCode));
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);
            };

            //费用上限变化
            playerItem.ttManaCostLimitChange = (int changeNum ) =>
            {
                SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, changeNum, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_LIMIT_CHANGE, playerItem.playerCode));
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);

            };
            //费用变化
            playerItem.ttManaCostUsableChange = (int changeNum) =>
            {
                SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, changeNum, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_USABLE_CHANGE, playerItem.playerCode));
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);

            };
            //增加科技
            playerItem.ttAddTraitType = (TraitType traitType) =>
            {
                SendNotification(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS, traitType, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_ADD, playerItem.playerCode));
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);

            };
            //分数变化
            playerItem.ttScoreChange = (int changeNum) =>
            {
                SendNotification(UIViewSystemEvent.UI_PLAYER_SCORE_SHOW_SYS, changeNum, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_PLAYER_SCORE_SHOW_SYS_CHANGE, playerItem.playerCode));
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, changeNum, StringUtil.GetNTByNotificationTypeAndPlayerCode(TimeTriggerEvent.TIME_TRIGGER_SYS_SCORE_CHANGE, playerItem.playerCode));
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
