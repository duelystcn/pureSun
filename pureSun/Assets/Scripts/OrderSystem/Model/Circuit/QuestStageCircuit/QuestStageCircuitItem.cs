

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using System.Collections.Generic;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit.QuestStageTimeTrigger;

namespace Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit
{
    public enum QuestTurnStageState
    {
        //开始一个阶段
        StartOfState,
        //执行一个阶段
        ExecutionOfState,
        //结束一个阶段
        EndOfState
    } 

    public class QuestStageCircuitItem
    {
        //回合数
        public int turnNum = 0;
        public string nowPlayerCode { get;  set; }
        public List<string> playerOrder { get; set; }

        public GM_OneStageSite oneTurnStage;

        public QuestTurnStageState questTurnStageState;

        public List<GM_OneStageSite> questOneTurnStageList =new List<GM_OneStageSite>();

        public TTOneStageStartAction oneStageStartAction;

        public TTOneStageEndAction oneStageEndAction;

        public TTOneStageExecutionAction oneStageExecutionAction;

        public TTOneTurnEndAction oneTurnEndAction;

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
        //根据code返回一个StageSite
        public GM_OneStageSite getOneStageSiteByStageCode(string stageCode) {
            foreach (GM_OneStageSite oneStageSite in questOneTurnStageList) {
                if (oneStageSite.code == stageCode) {
                    return oneStageSite;
                }
            }
            return null;
        }

    }
    
}
