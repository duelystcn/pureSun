

using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Database.Card;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public class PlayerTimeTrigger
    {
        //玩家需要抽了一张牌
        public delegate void TTPlayerNeedDrawACard();
        //玩家获得了一张牌
        public delegate void TTPlayerGetACard(CardEntry handCellItem);

        //玩家移除一张牌
        public delegate void TTPlayerRemoveACard(CardEntry handCellItem);
        //玩家使用一张牌
        public delegate void TTPlayerUseACard(CardEntry handCellItem);
        //判断手牌是否可用
        public delegate void TTPlayerHandCanUseJudge();



        //玩家可用费用发生了变化
        public delegate void TTManaCostChange(VariableAttribute manaVariableAttribute);

        //玩家增加了一点科技
        public delegate void TTAddTraitType(TraitType traitType);

        //玩家分数发生了变化
        public delegate void TTScoreChange(int changeNum);

      
    }
}
