

using Assets.Scripts.OrderSystem.Model.Database.Card;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    //这个类弃用了
    public class HandCellItem_Deprecated
    {
        //顺序标识
        public int locationIndex;
        //卡的实例
        public CardEntry cardEntry { get; private set; }
        //颜色
        public Color color;
        //uuid做唯一标识
        public string uuid;
        //判断是否可使用标识
        public bool canUse = false;



        public HandCellItem_Deprecated(CardEntry cardEntry)
        {
            this.cardEntry = cardEntry;
        }
    }
}
