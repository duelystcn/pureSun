

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
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

       


        public void EffectActionReady(EffectInfo effect) {
            foreach (string targetSet in effect.targetSet) {
                TargetSet targetSetDto = TransExpV2<TargetSet, TargetSet>.Trans(targetSetMap[targetSet]);
                effect.targetSetList.Add(targetSetDto);
                if (targetSetDto.target == "Minion")
                {
                    effect.TargetMinionList = (List<MinionCellItem> minionCellItemList) =>
                    {
                        for (int n = 0; n < minionCellItemList.Count; n++)
                        {
                            if (effect.effectiveTime == "Continued") {
                                minionCellItemList[n].effectBuffInfoList.Add(effect);
                                minionCellItemList[n].ttBuffChange();
                            }
                            for (int m = 0; m < effect.impactTargets.Length; m++)
                            {
                                ChangeMinion(effect.impactTargets[m], effect.impactContents[m], minionCellItemList[n]);
                            }
                        }
                    };
                }
                else if (targetSetDto.target == "Player")
                {
                    effect.TargetPlayerList = (List<PlayerItem> playerItemList) =>
                    {
                        for (int n = 0; n < playerItemList.Count; n++)
                        {
                            UtilityLog.Log("目标玩家【" + playerItemList[n].playerCode + "】生效效果【" + effect.description + "】", LogUtType.Effect);
                            for (int m = 0; m < effect.impactTargets.Length; m++)
                            {
                                ChangePlayer(effect.impactTargets[m], effect.impactContents[m], playerItemList[n]);
                            }
                        }
                    };
                }

            }
           

        }
        void ChangeMinion(string impactTarget, string impactContent, MinionCellItem minionCellItem)
        {
            switch (impactTarget)
            {
                case "ATK":
                    minionCellItem.atkNow = minionCellItem.atkNow + Convert.ToInt32(impactContent);
                    minionCellItem.ttAtkChange(Convert.ToInt32(impactContent));
                    break;
                case "DEF":
                    minionCellItem.defNow = minionCellItem.defNow + Convert.ToInt32(impactContent);
                    minionCellItem.ttDefChange(Convert.ToInt32(impactContent));
                    break;
                case "Attack":
                    minionCellItem.ttLaunchAnAttack();
                    break;
            }
        }
        void ChangePlayer(string impactTarget, string impactContent, PlayerItem playerItem)
        {
            switch (impactTarget)
            {
                case "Hand":
                    playerItem.DrawCard(Convert.ToInt32(impactContent));
                    break;
                //资源上限
                case "ManaUpperLimit":
                    playerItem.ChangeManaUpperLimit(Convert.ToInt32(impactContent));
                    break;
                case "ManaUsable":
                    //恢复至上限使用Max,是不能被转为数字的
                    if (impactContent == "Max")
                    {
                        playerItem.RestoreToTheUpperLimit();
                    }
                    else {
                        playerItem.ChangeManaUsable(Convert.ToInt32(impactContent));
                    }
                    
                    break;
                //科技相关
                case "TraitAddOne":
                    playerItem.AddTraitType(impactContent);
                    break;
                case "Score":
                    playerItem.ChangeSocre(Convert.ToInt32(impactContent));
                    break;
                case "CanUseResourceNum":
                    if (impactContent == "Max")
                    {
                        // 使用资源牌恢复到最大次数
                        playerItem.RestoreCanUseResourceNumMax();
                    }
                    else
                    {
                        
                    }
                    break;

            }

           

        }
    }
}
