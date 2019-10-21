
namespace Assets.Scripts.OrderSystem.Model.Player.PlayerAI
{
    public class PlayerItemAI
    {
        public void JudgeUseHandCard(PlayerItem playerItem) {
            //进行一次手牌可使用状态的检查
            playerItem.ChangeHandCardCanUse();

        }
    }
}
