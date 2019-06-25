using Assets.Scripts.OrderSystem.Common.UnityExpand;
using System.Collections.Generic;

namespace Assets.Scripts.FSMsys
{
    public  class FSMBaseSys
    {
        //当前状态
        public FSMBaseState nowState;
        //状态Map
        private Dictionary<string, FSMBaseState> stateDictionary = new Dictionary<string, FSMBaseState>();
        //事件Map
        private Dictionary<string, FSMEvent> eventDictionary = new Dictionary<string, FSMEvent>();

        //添加一个状态
        public void AddFSMState(FSMBaseState state)
        {
            if (state == null)
            {
                UtilityLog.LogError("状态为空");
                return;
            }
            if (nowState == null)
            {
                //状态运行
                nowState = state;
                nowState.StartState();
            }
            if (stateDictionary.ContainsKey(state.name))
            {
                UtilityLog.LogError("状态在状态机中已存在");
                return;
            }
            stateDictionary.Add(state.name, state);
        }
        //删除一个状态
        public void DeleteFSMState(FSMBaseState state)
        {
            if (state == null)
            {
                UtilityLog.LogError("状态为空");
                return;
            }
            if (!stateDictionary.ContainsKey(state.name))
            {
                UtilityLog.LogError("状态在状态机中不存在");
                return;
            }
            stateDictionary.Remove(state.name);
        }
        //检测状态是否存在
        public bool CheckFSMState(FSMBaseState state)
        {
            if (stateDictionary.ContainsKey(state.name))
            {
                return true;
            }
            else {
                return false;
            }
        }
        //录入事件前需要先录入状态
        public void AddFSMEvent(FSMEvent fSMEvent) {
            if (fSMEvent == null)
            {
                UtilityLog.LogError("事件为空");
                return;
            }
            //检测状态是否存在
            if (!CheckFSMState(fSMEvent.from) || !CheckFSMState(fSMEvent.to))
            {
                UtilityLog.LogError("事件状态在状态机中不存在");
                return;
            }
            if (eventDictionary.ContainsKey(fSMEvent.code)) {
                UtilityLog.LogError("事件在状态机中已存在");
                return;
            }
            eventDictionary.Add(fSMEvent.code,fSMEvent);
        }


    }
}
  
