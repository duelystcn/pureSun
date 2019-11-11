

using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.EffectCompent
{
    public class EffectOperationalTarget
    {
        //谁来选
        public string whoOperate { get; set; }
        //选择内容
        public string[] selectTarget { get; set; }

        public List<TargetSet> selectTargetList = new List<TargetSet>();

        public void CleanEffectTargetSetList() {
            foreach(TargetSet targetSet in selectTargetList)
            {
                targetSet.CleanEffectTargetSetList();
            }
        }
    }
}
