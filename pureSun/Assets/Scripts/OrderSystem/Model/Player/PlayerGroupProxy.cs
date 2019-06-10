

using Assets.Scripts.OrderSystem.Model.Hex;
using PureMVC.Patterns.Proxy;
using UnityEngine;

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
                Debug.LogWarning("玩家信息为空");
            }
            return returnItem;
        }
        

    }
}
