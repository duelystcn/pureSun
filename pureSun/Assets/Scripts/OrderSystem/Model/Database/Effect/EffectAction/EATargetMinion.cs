using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Minion;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction
{
    public class EATargetMinion
    {
        
        public delegate void EATargetMinionList(List<CardEntry> minionCellItemList, TargetSet objectSet);
    }
}
