

using Assets.Scripts.OrderSystem.Model.Database.Card;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public class PlayerTimeTrigger
    {
        //玩家抽了一张牌
        public delegate void TTPlayerDrawACard();
        //玩家获得了一张牌
        public delegate void TTPlayerGetACard(CardEntry handCellItem);

        //玩家移除一张牌
        public delegate void TTPlayerRemoveACard(CardEntry handCellItem);
        //玩家使用一张牌
        public delegate void TTPlayerUseACard(CardEntry handCellItem);
        //判断手牌是否可用
        public delegate void TTPlayerHandCanUseJudge();


        //玩家费用上限发生了变化
        public delegate void TTManaCostLimitChange(int changeNum);
        //玩家可用费用发生了变化
        public delegate void TTManaCostUsableChange(int changeNum);

        //玩家增加了一点科技
        public delegate void TTAddTraitType(TraitType traitType);

        //玩家分数发生了变化
        public delegate void TTScoreChange(int changeNum);

      
    }
}
