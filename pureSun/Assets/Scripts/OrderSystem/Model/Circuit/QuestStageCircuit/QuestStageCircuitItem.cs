

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit
{
    /** public enum QuestOneTurnStage
    {
        //开始阶段
        StartOfTrun,
        //主动阶段
        ActivePhase,
        //对手守备
        OpponentDefense,
        //延迟执行
        DelayedExecution,
        //战斗开始
        StartOfBattle
    } **/

    public class QuestStageCircuitItem
    {
        public int turnNum { get;  set; }
        public string nowPlayerCode { get;  set; }
        public List<string> playerOrder { get; set; }

        public string oneTurnStage;

        public List<string> questOneTurnStageList =new List<string>();

        public UnityAction oneStageStartAction;

        public UnityAction oneStageEndAction;

        public bool autoNextStage = false;


        public Dictionary<string, List<EffectInfo>>  activeEffectInfoMap = new Dictionary<string, List<EffectInfo>>();

        public void putOneEffectInfoInActiveMap(EffectInfo effectInfo, Dictionary<string, ImpactTimeTrigger> impactTimeTriggerMap) {
            foreach (string impactTimeTriggerStr in effectInfo.impactTimeTriggers) {
                ImpactTimeTrigger impactTimeTrigger = impactTimeTriggerMap[impactTimeTriggerStr];
                effectInfo.impactTimeTriggerList.Add(impactTimeTrigger);
                if (activeEffectInfoMap.ContainsKey(impactTimeTrigger.impactTimeTriggertMonitor))
                {
                    activeEffectInfoMap[impactTimeTrigger.impactTimeTriggertMonitor].Add(effectInfo);
                }
                else {
                    List<EffectInfo> effectInfos = new List<EffectInfo>();
                    effectInfos.Add(effectInfo);
                    activeEffectInfoMap.Add(impactTimeTrigger.impactTimeTriggertMonitor, effectInfos);
                }
            }

        }

    }
    
}
