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
        public PlayerItem AddHumanPlayer(string playerCode) {
            PlayerItem playerItem = new PlayerItem(playerCode);
            playerItem.playerType = PlayerType.HumanPlayer;
            playerItems.Add(playerCode,playerItem);
            return playerItem;
        }

        public PlayerItem AddAIPlayer(string playerCode)
        {
            PlayerItem playerItem = new PlayerItem(playerCode);
            playerItem.playerType = PlayerType.AIPlayer;
            playerItems.Add(playerCode, playerItem);
            return playerItem;
        }



    }
}
