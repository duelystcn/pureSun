using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Player
{
    //用户组
    public class PlayerGroup
    {
        public Dictionary<string, PlayerItem> playerItems
        {
            get; private set;
        }
        public void Create() {
            playerItems = new Dictionary<string, PlayerItem>();
        }
        public void AddPlayer(string playerCode) {
            PlayerItem playerItem = new PlayerItem(playerCode);
            playerItems.Add(playerCode,playerItem);
        }
      


    }
}
