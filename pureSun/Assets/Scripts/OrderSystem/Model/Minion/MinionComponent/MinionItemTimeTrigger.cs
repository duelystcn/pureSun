using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Model.Minion.MinionComponent
{
    public class MinionItemTimeTrigger
    {
        //生物攻击发生了变化
        public delegate void TTAtkChange(int changeNum);
        //生物生命发生了变化
        public delegate void TTDefChange(int changeNum);
        //生物受到伤害


        //生物的buff发生了变化
        public delegate void TTBuffChange();

        //生物发起一次攻击,，不是时点，是通知效果执行器来发起攻击
        public delegate void TTLaunchAnAttack();

        //生物进行一次攻击
        public delegate void TTExecuteAnAttack();

        //生物的buff需要被移除
        public delegate void TTBuffNeedRemove();
    }
}
