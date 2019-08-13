

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
        /// 监听器相关，需要重新渲染手牌可用，暂时放在这里
        /// </summary>
        public const string TIME_TRIGGER_SYS_HAND_CAN_USE_JUDGE = "TimeTriggerSysHandCanUseJudge";


    }
}
