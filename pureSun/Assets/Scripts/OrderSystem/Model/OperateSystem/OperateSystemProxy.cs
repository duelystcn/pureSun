

using Assets.Scripts.OrderSystem.Model.Hand;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Patterns.Proxy;

namespace Assets.Scripts.OrderSystem.Model.OperateSystem
{
    public class OperateSystemProxy : Proxy
    {
        public new const string NAME = "OperateSystemProxy";
        public HexModelInfo hexModelInfo;
        public OperateSystemItem operateSystemItem
        {
            get { return (OperateSystemItem)base.Data; }
        }
        public OperateSystemProxy(HexModelInfo hexModelInfo) : base(NAME)
        {
            OperateSystemItem operateSystemItem = new OperateSystemItem();
            this.hexModelInfo = hexModelInfo;
            base.Data = operateSystemItem;
        }
        //进入模式，使用手牌模式
        public void IntoModeHandUse (HandCellItem handCellItem, PlayerItem playerItem) {
            operateSystemItem.operateModeType = OperateSystemItem.OperateType.HandUse;
            operateSystemItem.onChooseHandCellItem = handCellItem;
            operateSystemItem.playerItem = playerItem;
        }
        //进入初始模式
        public void IntoModeClose() {
            operateSystemItem.operateModeType = OperateSystemItem.OperateType.Close;
            operateSystemItem.onChooseHandCellItem = null;
            operateSystemItem.playerItem = null;
        }

       

    }
}
