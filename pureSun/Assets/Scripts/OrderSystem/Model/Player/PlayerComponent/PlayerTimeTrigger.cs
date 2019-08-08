

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public class PlayerTimeTrigger
    {
        //玩家抽了一张牌
        public delegate void TTPlayerDrawACard(HandCellItem handCellItem);
        //玩家移除一张牌
        public delegate void TTPlayerRemoveACard(HandCellItem handCellItem);

    }
}
