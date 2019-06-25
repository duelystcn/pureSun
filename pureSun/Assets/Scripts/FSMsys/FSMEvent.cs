

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using UnityEngine;

namespace Assets.Scripts.FSMsys
{
    public class FSMEvent
    {
        public string code
        {
            get;private set;
        }
        public FSMBaseState from
        {
            get; private set;
        }
        public FSMBaseState to
        {
            get; private set;
        }
        public FSMEvent(string code, FSMBaseState from, FSMBaseState to) {
            this.code = code;
            this.from = from;
            this.to = to;
        }

        public void Action(FSMBaseSys fSMBaseSys) {
            //检测状态是否存在
            if (!fSMBaseSys.CheckFSMState(from)||!fSMBaseSys.CheckFSMState(to)) {
                UtilityLog.LogError("状态在状态机中不存在");
                return;
            }
            if (fSMBaseSys.nowState.name != from.name) {
                UtilityLog.LogError("开始状态错误");
                return;
            }
            fSMBaseSys.nowState.EndState();
            fSMBaseSys.nowState = from;
            fSMBaseSys.nowState.StartState();
        }

    }
}
