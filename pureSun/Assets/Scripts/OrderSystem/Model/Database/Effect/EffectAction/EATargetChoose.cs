

using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.SpecialOperate.ChooseOperate;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction
{
    public class EATargetChoose
    {
        public delegate void EATargetChooseGrid(ChooseGridItem chooseGridItem, TargetSet objectSet);
    }
}
