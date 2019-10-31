
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using System.Collections.Generic;
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
        //所在位置
        public int index;
        //当前攻击
        public int atkNow;
        //当前生命
        public int defNow;
        //累计受到的伤害
        public int cumulativeDamage = 0;

        //时点触发器
        //生物攻击发生了变化
        public TTAtkChange ttAtkChange;
        //生物生命发生了变化
        public TTDefChange ttDefChange;
        //生物Buff发生了变化,目前好像不会触发这方面的效果，主要是触发界面显示变化
        public TTBuffChange ttBuffChange;


        //持续buff，放在一个list里
        public List<EffectInfo> effectBuffInfoList = new List<EffectInfo>();

    }
}
