
using Assets.Scripts.OrderSystem.Model.Database.Card;
using UnityEngine;
using static Assets.Scripts.OrderSystem.Model.Minion.MinionComponent.MinionItemTimeTrigger;

namespace Assets.Scripts.OrderSystem.Model.Minion
{
    public class MinionCellItem
    {
        //颜色
        public Color color;
        //卡实例
        public CardEntry cardEntry;
        //是否可作为效果对象
        public bool IsEffectTarget;
        //uuid
        public string uuid;
        //所有者
        public string playerCode;

        //时点触发器
        //生物攻击发生了变化
        public TTAtkChange ttAtkChange;
        //生物生命发生了变化
        public TTDefChange ttDefChange;

    }
}
