using PureMVC.Patterns.Proxy;

namespace Assets.Scripts.OrderSystem.Model.Minion
{
    public class MinionGridProxy : Proxy
    {
        public new const string NAME = "MinionGridProxy";

        public MinionGridItem minionGridItem
        {
            get { return (MinionGridItem)base.Data; }
        }

        public MinionGridProxy() : base(NAME)
        {
            MinionGridItem minionGridItem = new MinionGridItem();
            minionGridItem.Create();
            base.Data = minionGridItem;
        }
        public MinionCellItem GetMinionCellItemByIndex(int index) {
            return minionGridItem.minionCells[index];

        }
    }
}
