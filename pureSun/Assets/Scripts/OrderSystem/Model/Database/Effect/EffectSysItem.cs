

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect.EffectCompent;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.SpecialOperate.ChooseOperate;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public enum EffectSysItemStage
    {
        //未开始
        UnStart,
        //执行中
        Executing
    }

    public class EffectSysItem
    {
        public EffectSysItemStage effectSysItemStage = EffectSysItemStage.UnStart;

        //效果信息
        public Dictionary<string, EffectInfo> effectInfoMap;
        //触发器信息
        public Dictionary<string, ImpactTimeTrigger> impactTimeTriggerMap;
        //目标信息
        public Dictionary<string, TargetSet> targetSetMap;

        //效果结算？写在这里吧，从操作模式中移动到这里
        //正在结算的卡
        public CardEntry cardEntry;


        //正在结算的效果集合
        public List<EffectInfo> effectInfos;


  
        //如果正在结算的效果是同步结算，那么应该把这期间所触发的时点全部延后处理
        public Queue<INotification> delayNotifications = new Queue<INotification>();

        //结算卡队列
        public Queue<CardEntry> cardEntryQueue = new Queue<CardEntry>();
        //结算效果集合队列
        public Queue<List<EffectInfo>> effectInfosQueue = new Queue<List<EffectInfo>>();

        //发送到前台进行展示的效果个数
        public int showEffectNum = 0;


        public void EffectActionReady(EffectInfo effect)
        {
            effect.operationalTarget.selectTargetList = new List<TargetSet>();
            foreach (string targetSetCode in effect.operationalTarget.selectTarget)
            {
                TargetSet targetSetDto = TransExpV2<TargetSet, TargetSet>.Trans(targetSetMap[targetSetCode]);
                targetSetDto.CleanEffectTargetSetList();
                effect.operationalTarget.selectTargetList.Add(targetSetDto);
            }
        }
    }
}
