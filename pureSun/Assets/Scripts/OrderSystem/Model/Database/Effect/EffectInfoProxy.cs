
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
        //根据目标对象的名称获取一个深度克隆的目标对象信息
        public TargetSet GetDepthCloneTargetSetByName(string targetSetCode) {
            return TransExpV2<TargetSet, TargetSet>.Trans(effectSysItem.targetSetMap[targetSetCode]);
            
        }
        //根据效果的名称获取一个深度克隆的效果信息
        public EffectInfo GetDepthCloneEffectByName(string effectCode) {
            EffectInfo effectInfo = TransExpV2<EffectInfo, EffectInfo>.Trans(effectSysItem.effectInfoMap[effectCode]);
            //初始化效果
            effectSysItem.EffectActionReady(effectInfo);
            if (effectInfo.preEffectList != null) {
                //实例化每一个前置后置效果
                foreach (string preEffectCode in effectInfo.preEffectList)
                {
                    EffectInfo preEffectInfo = TransExpV2<EffectInfo, EffectInfo>.Trans(effectSysItem.effectInfoMap[preEffectCode]);
                    effectSysItem.EffectActionReady(preEffectInfo);
                    effectInfo.preEffectEntryList.Add(preEffectInfo);
                }
            }
            if (effectInfo.postEffectList != null) {
                //实例化每一个前置后置效果
                foreach (string postEffectCode in effectInfo.postEffectList)
                {
                    EffectInfo postEffectInfo = TransExpV2<EffectInfo, EffectInfo>.Trans(effectSysItem.effectInfoMap[postEffectCode]);
                    effectSysItem.EffectActionReady(postEffectInfo);
                    effectInfo.postEffectEntryList.Add(postEffectInfo);
                }
            }
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
                    UtilityLog.Log("效果系统取出了卡牌【" + effectSysItem.cardEntry.name + "】进行释放", LogUtType.Effect);
                    //SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_OBJECT);
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
