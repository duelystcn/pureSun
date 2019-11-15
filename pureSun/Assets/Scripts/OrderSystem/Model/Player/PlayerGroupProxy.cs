

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Database.Card;
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
            playerItem.ttPlayerNeedDrawACard = () =>
            {
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, playerItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(TimeTriggerEvent.TIME_TRIGGER_SYS_NEED_DRAW_A_CARD, playerItem.playerCode));
                playerItem.ttPlayerGetACard(null);
            };
            //获得一张牌
            playerItem.ttPlayerGetACard = (CardEntry handCellItem) =>
            {
                //SendNotification(UIViewSystemEvent.UI_VIEW_ZF_HAND_CHANGE, handCellItem, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_DRAW_ONE_CARD, playerItem.playerCode));
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);

            };
            //判断手牌是否可用
            playerItem.ttPlayerHandCanUseJudge = () =>
            {
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);
            };
            //使用一张牌
            playerItem.ttPlayerUseACard = (CardEntry handCellItem) =>
            {
                SendNotification(OperateSystemEvent.OPERATE_SYS, playerItem.playerCode, OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE);
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, handCellItem, TimeTriggerEvent.TIME_TRIGGER_SYS_USE_HAND_CARD);
            };
            //费用变化
            playerItem.ttManaCostChange = (VariableAttribute manaVariableAttribute) =>
            {
                SendNotification(UIViewSystemEvent.UI_MANA_INFA_SYS, VariableAttributeMap.CopyOneVariableAttribute(manaVariableAttribute), StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_MANA_INFA_SYS_NUM_CHANGE, playerItem.playerCode));
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
        //获取一个玩家的对立玩家，暂时直接返回不是这个玩家的
        public PlayerItem getEnemytPlayerByPlayerCode(string playerCode)
        {
            foreach (PlayerItem playerItem in playerGroup.playerItems.Values)
            {
                if (playerItem.playerCode != playerCode)
                {
                    return playerItem;
                }
            }
            return null;
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
