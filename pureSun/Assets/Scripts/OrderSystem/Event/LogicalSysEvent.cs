using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Event
{
    public class LogicalSysEvent
    {
        //逻辑判断相关
        public const string LOGICAL_SYS = "LogicalSys";

        //选择船只卡牌
        public const string LOGICAL_SYS_CHOOSE_SHIP_CARD = "LogicalSysChooseShipCard";

        //需要选择效果
        public const string LOGICAL_SYS_CHOOSE_EFFECT = "LogicalSysChooseEffect";

        //AI玩家主回合操作
        public const string LOGICAL_SYS_ACTIVE_PHASE_ACTION = "LogicalSysActivePhaseAction";
    }
}
