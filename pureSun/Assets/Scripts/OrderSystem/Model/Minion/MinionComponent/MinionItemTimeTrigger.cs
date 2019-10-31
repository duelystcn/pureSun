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
        //生成了一个生物，添加到页面
        public delegate void TTBuffChange();
    }
}
