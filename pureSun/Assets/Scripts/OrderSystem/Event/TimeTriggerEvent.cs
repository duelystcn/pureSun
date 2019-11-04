

namespace Assets.Scripts.OrderSystem.Event
{
    class TimeTriggerEvent
    {
        /// <summary>
        /// 监听器相关，监听到了变化
        /// </summary>
        public const string TIME_TRIGGER_SYS = "TimeTriggerSys";
        /// <summary>
        /// 监听器相关，监听到了手牌变化
        /// </summary>
        public const string TIME_TRIGGER_SYS_DRAW_A_CARD = "TimeTriggerSysDrawACard";
      
        /// <summary>
        /// 监听器相关，分数变化
        /// </summary>
        public const string TIME_TRIGGER_SYS_SCORE_CHANGE = "TimeTriggerSysScoreChange";
        /// <summary>
        /// 监听器相关，手牌使用
        /// </summary>
        public const string TIME_TRIGGER_SYS_USE_HAND_CARD = "TimeTriggerSysUseHandCard";





        /// <summary>
        /// 一个阶段的开始
        /// </summary>
        public const string TIME_TRIGGER_SYS_ONE_STAGE_START = "TimeTriggerSysOneStageStart";
        /// <summary>
        /// 一个阶段的结束
        /// </summary>
        public const string TIME_TRIGGER_SYS_ONE_STAGE_END = "TimeTriggerSysOneStageEnd";
        /// <summary>
        /// 一个阶段的执行
        /// </summary>
        public const string TIME_TRIGGER_SYS_ONE_STAGE_EXECUTION = "TimeTriggerSysOneStageExecution";

        /// <summary>
        /// 一个回合的结束
        /// </summary>
        public const string TIME_TRIGGER_SYS_ONE_TURN_END = "TimeTriggerSysOneTurnEnd";




        /// <summary>
        /// 执行下一个被延迟处理的时点
        /// </summary>
        public const string TIME_TRIGGER_EXE_NEXT_DELAY_NOTIFICATION = "TimeTriggerExeNextDelayNotification";

    }
}
