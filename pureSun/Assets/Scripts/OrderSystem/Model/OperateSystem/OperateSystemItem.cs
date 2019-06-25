using Assets.Scripts.OrderSystem.Model.Hand;
using Assets.Scripts.OrderSystem.Model.Player;

namespace Assets.Scripts.OrderSystem.Model.OperateSystem
{
    public class OperateSystemItem
    {
        public enum OperateType { Close, HandUse };
        public OperateType operateModeType = OperateType.Close;
        public HandCellItem onChooseHandCellItem
        {
            get;  set;
        }
        //当前操作用户
        public PlayerItem playerItem
        {
            get;  set;
        }
       
    }
}
