
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.Model.Player.PlayerComponent.PlayerTimeTrigger;

namespace Assets.Scripts.OrderSystem.Model.Player
{
    public enum PlayerType { HumanPlayer, AIPlayer, NetPlayer };
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

        //费用相关
        public ManaItem manaItem;


        public PlayerItem(string playCode)
        {
            this.playerCode = playCode;
            handGridItem = new HandGridItem();
            handGridItem.Create();
            cardDeck = new CardDeck();
            manaItem = new ManaItem();
        }

        //时点触发器
        //抽一张牌
        public TTPlayerDrawACard ttPlayerDrawACard;
        //移除一张牌
        public TTPlayerRemoveACard ttPlayerRemoveACard;

        //方法抽一张牌
        public void DrawCard(int num) {
            for (int n = 0; n < num; n++) {
                HandCellItem handcellItem = this.handGridItem.CreateCell(this.cardDeck.GetFirstCard());
                ttPlayerDrawACard(handcellItem);
            }
        }
        //移除一张手牌
        public void RemoveOneCard(HandCellItem handCellItem) {
            //找到目标要移除的牌
            HandCellItem targetHand = null;
            foreach (HandCellItem handCell in handGridItem.handCells) {
                if (handCell.cardEntry.uuid == handCellItem.cardEntry.uuid) {
                    targetHand = handCell;
                }
            }
            if (targetHand != null)
            {
                handGridItem.handCells.Remove(targetHand);
                ttPlayerRemoveACard(targetHand);
            }
            else {
                UtilityLog.LogError("找不到要移除的手牌");

            }
           
        }
    }


}
