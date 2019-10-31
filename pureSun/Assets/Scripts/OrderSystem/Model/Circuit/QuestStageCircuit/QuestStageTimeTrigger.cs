using Assets.Scripts.OrderSystem.Model.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit
{
    public class QuestStageTimeTrigger
    {
        //一个阶段的开始
        public delegate void TTOneStageStartAction(PlayerItem playerItem);
        //一个阶段的执行
        public delegate void TTOneStageExecutionAction(PlayerItem playerItem);
        //一个阶段的开始
        public delegate void TTOneStageEndAction(PlayerItem playerItem);
    }
}
