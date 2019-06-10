using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.FSMsys
{
    public abstract class FSMBaseState
    {
        public string name;
        //状态开始时
        public void StartState() {

        }
        //状态结束时
        public void EndState() {

        }
        
    }
}
