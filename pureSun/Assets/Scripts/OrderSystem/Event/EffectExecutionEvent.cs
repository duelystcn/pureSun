using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Event
{
    public class EffectExecutionEvent
    {
        //手牌操作
        public const string EFFECT_EXECUTION_SYS = "EffectExecutionSys";
        //执行一张牌的效果
        public const string EFFECT_EXECUTION_SYS_EXE_CARD = "EffectExecutionSysExeCard";

        //已被触发，执行一个效果
        public const string EFFECT_EXECUTION_SYS_EXE_TRIGGERED_CARD = "EffectExecutionSysExeTriggeredCard";

        //执行效果寻找目标
        public const string EFFECT_EXECUTION_SYS_FIND_TARGET = "EffectExecutionSysFindTarget";

        //执行效果寻找目标
        public const string EFFECT_EXECUTION_SYS_EXE_EFFECT = "EffectExecutionSysExeEffect";
    }
}
