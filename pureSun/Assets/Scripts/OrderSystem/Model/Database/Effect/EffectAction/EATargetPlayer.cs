using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Player;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction
{
    public class EATargetPlayer
    {
        public delegate bool EATargetPlayerList(List<PlayerItem> playerItemList , TargetSet objectSet, EffectExeType effectExeType);
    }
}
