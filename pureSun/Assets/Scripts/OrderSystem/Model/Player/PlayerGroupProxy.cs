

using Assets.Scripts.OrderSystem.Common.UnityExpand;
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
        public void AddPlayer() {
           
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
