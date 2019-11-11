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

        //执行效果寻找宾语目标
        public const string EFFECT_EXECUTION_SYS_FIND_OBJECT = "EffectExecutionSysFindObejct";

        //执行效果寻找主语目标
        public const string EFFECT_EXECUTION_SYS_FIND_TARGET = "EffectExecutionSysFindTarget";

        //询问用户是否需要确认效果发动或者选择
        public const string EFFECT_EXECUTION_SYS_EXE_EFFECT = "EffectExecutionSysExeEffect";

        //执行效果
        public const string EFFECT_EXECUTION_SYS_ASK_THE_USER = "EffectExecutionSysAskTheUser";

        //一个效果执行完毕，用来执行动画

        //展示完毕
        public const string EFFECT_EXECUTION_SYS_EFFECT_SHOW_OVER = "EffectExecutionSysShowOver";

        //生物发起一次攻击，暂时先写在这里
        public const string EFFECT_EXECUTION_SYS_LAUNCH_AN_ATTACK = "EffectExecutionSysLaunchAnAttack";

        //生物发起一次移动，暂时先写在这里
        public const string EFFECT_EXECUTION_SYS_LAUNCH_AN_MOVE = "EffectExecutionSysLaunchAnMove";

        //生物的buff需要被移除
        public const string EFFECT_EXECUTION_SYS_MINION_BUFF_NEED_REMOVE = "EffectExecutionSysMinionBuffNeedRemove";

        //生物进入战场，检查是否有效果需要触发
        public const string EFFECT_EXECUTION_SYS_MINION_ENTER_THE_BATTLEFIELD = "EffectExecutionSysMinionEnterTheBattlefield";

    }
}
