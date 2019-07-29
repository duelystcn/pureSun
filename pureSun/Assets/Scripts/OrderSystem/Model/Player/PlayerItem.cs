
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;

namespace Assets.Scripts.OrderSystem.Model.Player
{
    public enum PlayerType { HumanPlayer, AIPlayer };
    public class PlayerItem
    {

        //主要ID
        public string playerCode
        {
            get; private set;
        }
        public PlayerType playerType;
        //手牌
        public HandGridItem handGridItem;

        //船
        public CardEntry shipCard;


        //牌组
        public CardDeck cardDeck ;


        //起始点，虚拟坐标，用于确认召唤范围？
        public HexCoordinates hexCoordinates;


        public PlayerItem(string playCode)
        {
            this.playerCode = playCode;
            handGridItem = new HandGridItem();
            handGridItem.Create();
            cardDeck = new CardDeck();
        }

    }


}
