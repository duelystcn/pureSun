
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Common.BasicGame;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.OrderSystem.Model.Minion.MinionComponent.MinionItemTimeTrigger;

namespace Assets.Scripts.OrderSystem.Model.Minion
{
    public enum MinionComeFrom { Hand, Graveyard, Effect };
    public class MinionCellItem: BasicGameDto
    {
        //颜色
        public Color color;
        //卡实例
        public CardEntry cardEntry;
        //是否可作为效果对象
        public bool IsEffectTarget;

        //所在位置
        // public int index;
        public HexCoordinates index;
        //原始所在位置，演算移动动画时需要用到
        public HexCoordinates lastIndex;

        //当前攻击
        //public int atkNow;
        //当前生命
        //public int defNow;

        //费用，攻击，生命等可变属性保存
        public VariableAttributeMap minionVariableAttributeMap = new VariableAttributeMap();

        //生物产生途径？
        public MinionComeFrom minionComeFrom;



        //累计受到的伤害
        //public int cumulativeDamage = 0;

        //攻击的目标点，前端页面播放时需要用到
        public HexCoordinates attackTargetIndex;

        //时点触发器
        //生物攻击发生了变化
        public TTAtkChange ttAtkChange;
        //生物生命发生了变化
        public TTDefChange ttDefChange;
        //生物Buff发生了变化,目前好像不会触发这方面的效果，主要是触发界面显示变化
        public TTBuffChange ttBuffChange;
        //生物Buff需要取消，将其发送到效果执行器进行取消
        public TTBuffNeedRemove ttBuffNeedRemove;
        //生物发起攻击
        public TTLaunchAnAttack ttLaunchAnAttack;
        //生物进行攻击
        public TTExecuteAnAttack ttExecuteAnAttack;

        //生物发起移动
        public TTLaunchAnMove ttLaunchAnMove;
        //生物进行移动
        public TTExecuteAnMove ttExecuteAnMove;

        //生物死亡
        public TTMinionIsDead ttMinionIsDead;

        //生物进入战场
        public TTMinionIntoBattlefield ttMinionIntoBattlefield;






        //持续buff，放在一个list里
        public List<EffectInfo> effectBuffInfoList = new List<EffectInfo>();
        //生物受到伤害
        public void SufferDamage(int damageNum) {
            //cumulativeDamage += damageNum;

            minionVariableAttributeMap.ChangeValueByCodeAndType("Def", VATtrtype.DamageValue, -damageNum);
            ttDefChange(damageNum);
            CheckMinionIsDead();
        }
        //检查生物是否死亡
        public void CheckMinionIsDead() {
            if (minionVariableAttributeMap.GetValueByCodeAndType("Def", VATtrtype.CalculatedValue) <= 0) {
                ttMinionIsDead();
            }
        
        }

        //生物攻击某一个生物
        public void AttackTargetMinion(MinionCellItem defensiveMinionCellItem)
        {
            this.attackTargetIndex = defensiveMinionCellItem.index;
            ttExecuteAnAttack();
            defensiveMinionCellItem.SufferDamage(minionVariableAttributeMap.GetValueByCodeAndType("Atk", VATtrtype.CalculatedValue));
        }
        //生物反击某一个生物
        public void CounterAttackTargetMinion(MinionCellItem attackMinionCellItem) {
            this.attackTargetIndex = attackMinionCellItem.index;
            ttExecuteAnAttack();
            attackMinionCellItem.SufferDamage(minionVariableAttributeMap.GetValueByCodeAndType("Atk", VATtrtype.CalculatedValue));
        }
      

        //检查是否需要清除buff
        public void CheckNeedChangeEffectBuffInfo(string timeTrigger) {
            bool buffHasChange = false;
            foreach (EffectInfo effectInfo in effectBuffInfoList) {
                //倒计时为0则清除buff
                if (effectInfo.effectiveTime.ContinuousStage == timeTrigger) {
                    effectInfo.effectiveTime.ContinuousRound--;
                    if (effectInfo.effectiveTime.ContinuousRound == 0) {
                        buffHasChange = true;
                    }
                }
               
            }
            if (buffHasChange) {
                ttBuffNeedRemove();
                //清除失效buff
                List<EffectInfo> newEffectBuffInfoList = new List<EffectInfo>();
                foreach (EffectInfo effectInfo in effectBuffInfoList)
                {
                    if (effectInfo.effectiveTime.ContinuousRound > 0) {
                        newEffectBuffInfoList.Add(effectInfo);
                    }
                }
                effectBuffInfoList = newEffectBuffInfoList;
                ttBuffChange();
            }
        }



    }
}
