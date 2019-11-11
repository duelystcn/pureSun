using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.OperateSystem
{
    public class OperateSystemProxy : Proxy
    {
        public new const string NAME = "OperateSystemProxy";
        public OperateSystemItem operateSystemItem
        {
            get { return (OperateSystemItem)base.Data; }
        }
        public OperateSystemProxy() : base(NAME)
        {
            OperateSystemItem operateSystemItem = new OperateSystemItem();
            base.Data = operateSystemItem;
        }
        //进入模式，使用手牌模式
        public void IntoModeHandUse (CardEntry handCellItem, PlayerItem playerItem) {
            operateSystemItem.operateModeType = OperateSystemItem.OperateType.HandUse;
            operateSystemItem.onChooseHandCellItem = handCellItem;
            operateSystemItem.playerItem = playerItem;
        }
        //关闭模式？
        public void IntoModeClose() {
            operateSystemItem.operateModeType = OperateSystemItem.OperateType.Close;
            operateSystemItem.onChooseHandCellItem = null;
            operateSystemItem.playerItem = null;
        }
       




    }
}
