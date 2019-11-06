using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Player;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction
{
    public class EATargetCardEntry
    {
        public delegate void EATargetCardEntryList(List<CardEntry> cardEntries, TargetSet objectSet);
    }
}
