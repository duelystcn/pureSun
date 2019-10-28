﻿
using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Newtonsoft.Json;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public class EffectInfoProxy : Proxy
    {
        public new const string NAME = "EffectInfoProxy";
        public EffectSysItem effectSysItem
        {
            get { return (EffectSysItem)base.Data; }
        }
        public EffectInfoProxy() : base(NAME)
        {
            EffectSysItem effectSysItem = new EffectSysItem();
            base.Data = effectSysItem;
            LoadCardDbByJson();
        }
        //读取JSON文件配置
        public void LoadCardDbByJson()
        {
            string effectSysStr = File.ReadAllText("Assets/Resources/Json/EffectDb.json", Encoding.GetEncoding("gb2312"));
            effectSysItem.effectInfoMap =
                JsonConvert.DeserializeObject<Dictionary<string, EffectInfo>>(effectSysStr);

            string impactTimeTriggerStr = File.ReadAllText("Assets/Resources/Json/ImpactTimeTrigger.json", Encoding.GetEncoding("gb2312"));
            effectSysItem.impactTimeTriggerMap =
                JsonConvert.DeserializeObject<Dictionary<string, ImpactTimeTrigger>>(impactTimeTriggerStr);

            string targetSetDbStr = File.ReadAllText("Assets/Resources/Json/TargetSetDb.json", Encoding.GetEncoding("gb2312"));
            effectSysItem.targetSetMap =
                JsonConvert.DeserializeObject<Dictionary<string, TargetSet>>(targetSetDbStr);


        }

        public EffectInfo GetDepthCloneEffectByName(string effectName) {
            EffectInfo effectInfo = TransExpV2<EffectInfo, EffectInfo>.Trans(effectSysItem.effectInfoMap[effectName]);
            //初始化效果
            effectSysItem.EffectActionReady(effectInfo);
            return effectInfo;
        }

        //cardEntry这个参数貌似不是必须的？
        //进入卡牌结算模式
        public void IntoModeCardSettle(CardEntry cardEntry, List<EffectInfo> effectInfos)
        {
            effectSysItem.effectInfosQueue.Enqueue(effectInfos);
            effectSysItem.cardEntryQueue.Enqueue(cardEntry);
            ExeEffectQueue();
        }
        public void ExeEffectQueue() {
            if (effectSysItem.effectSysItemStage == EffectSysItemStage.UnStart) {
                if (effectSysItem.cardEntryQueue.Count > 0)
                {
                    effectSysItem.effectSysItemStage = EffectSysItemStage.Executing;
                    effectSysItem.effectInfos = effectSysItem.effectInfosQueue.Dequeue();
                    effectSysItem.cardEntry = effectSysItem.cardEntryQueue.Dequeue();
                    SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_TARGET);
                }
                else {
                    //UtilityLog.Log("当前没有需要结算的效果");
                    //通知回合控制器当前堆叠已经全部执行完毕
                    //SendNotification(UIViewSystemEvent.UI_QUEST_TURN_STAGE, null, UIViewSystemEvent.UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE);
                }
            }
           

        }




    }
}
