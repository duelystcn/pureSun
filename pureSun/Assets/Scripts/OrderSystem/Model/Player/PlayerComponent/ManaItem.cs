

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public class ManaItem
    {
        //费用上限
        public int manaUpperLimit = 0;
        //可使用费用
        public int manaUsable = 0;

        public void changeManaUpperLimit(int num) {
            manaUpperLimit += num;
        }
        public void changeManaUsable(int num)
        {
            manaUsable += num;
        }
    }
}
