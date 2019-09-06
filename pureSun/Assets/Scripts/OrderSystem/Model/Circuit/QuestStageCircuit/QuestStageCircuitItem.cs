

using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit
{
    public enum QuestOneTurnStage
    {
        //未开始
        UnStart,
        //开始阶段
        StartOfTrun,
        //主动阶段
        ActivePhase,
        //对手守备
        OpponentDefense,
        //结束阶段
        EndOfTrun,
        //延迟执行
        DelayedExecution,
        //战斗开始
        StartOfBattle
    }

    public class QuestStageCircuitItem
    {
        public int turnNum { get;  set; }
        public string nowPlayerCode { get;  set; }
        public List<string> playerOrder { get; set; }

        public QuestOneTurnStage oneTurnStage = QuestOneTurnStage.UnStart;

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
