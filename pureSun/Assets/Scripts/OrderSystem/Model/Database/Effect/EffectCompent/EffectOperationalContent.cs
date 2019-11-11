using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.EffectCompent
{
    public class EffectOperationalContent
    {
        public string operationalTarget { get; set; }
        //影响目标
        public string[] impactTargets { get; set; }
        //影响内容
        //数组 和目标一一对应
        public string[] impactContents { get; set; }
    }
}
