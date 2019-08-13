
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

        //科技相关TraitCombination
        public TraitCombination traitCombination;



        public PlayerItem(string playCode)
        {
            this.playerCode = playCode;
            handGridItem = new HandGridItem();
            handGridItem.Create();
            cardDeck = new CardDeck();
            manaItem = new ManaItem();
            traitCombination = new TraitCombination();
        }

        //时点触发器
        //抽一张牌
        public TTPlayerDrawACard ttPlayerDrawACard;
        //移除一张牌
        public TTPlayerRemoveACard ttPlayerRemoveACard;

        //费用上限发生了变化
        public TTManaCostLimitChange ttManaCostLimitChange;
        //可用费用发生了变化
        public TTManaCostUsableChange ttManaCostUsableChange;

        //增加了科技
        public TTAddTraitType ttAddTraitType;

        public void AddTraitType(string traitName)
        {
            TraitType traitType = traitCombination.AddTraitType(traitName);
            ttAddTraitType(traitType);

        }


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
        //改变费用上限
        public void ChangeManaUpperLimit(int num)
        {
            manaItem.changeManaUpperLimit(num);
            ttManaCostLimitChange(num);
        }
        //改变可用费用
        public void ChangeManaUsable(int num)
        {
            manaItem.changeManaUsable(num);
            ttManaCostUsableChange(num);
        }
        //费用恢复至上限
        public void RestoreToTheUpperLimit()
        {
            int changeNum = manaItem.manaUpperLimit - manaItem.manaUsable;
            manaItem.RestoreToTheUpperLimit();
            ttManaCostUsableChange(changeNum);
        }
    }


}
