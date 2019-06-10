

using Assets.Scripts.OrderSystem.Model.Database.Card;

namespace Assets.Scripts.OrderSystem.Model.SpecialOperate.ChooseOperate
{
    public class ChooseGridItem
    {
        //需要选择几项
        public int chooseNum;
        //类别
        public enum ChooseType { CardChoose };

        public ChooseType WhichChoose;
        //选择对象列表
        public CardEntry[] chooseTargetList;

    }
}