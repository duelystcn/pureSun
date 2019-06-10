

using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.SpecialOperate.ChooseOperate;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public class EffectSysItem
    {
        public Dictionary<string, EffectInfo> effectInfoMap;

        public void EffectActionReady(EffectInfo effect) {
            if (effect.type == "ONE_MINION") {
                effect.TargetMinionOne += (MinionCellItem minionCellItem) =>
                {
                    for (int n = 0; n < effect.impactTargets.Length; n++) {
                        ChangeMinion(effect.impactTargets[n], effect.impactContents[n], minionCellItem);
                    }
                };
            }
            else if (effect.type == "CHOOSE_ONE") {
                effect.TargetChooseGrid += (ChooseGridItem chooseGridItem) =>
                {
                    CardEntry[] cardEntrys = new CardEntry[effect.chooseEffectList.Length];
                    for (int n = 0; n < effect.chooseEffectList.Length; n++) {
                        CardEntry cardEntry = new CardEntry();
                    }
                };

            }

        }
        void ChangeMinion(string impactTarget, string impactContent, MinionCellItem minionCellItem)
        {
            switch (impactTarget)
            {
                case "ATK":
                    minionCellItem.cardEntry.atk = minionCellItem.cardEntry.atk + Convert.ToInt32(impactContent);
                    break;
                case "DEF":
                    minionCellItem.cardEntry.def = minionCellItem.cardEntry.def + Convert.ToInt32(impactContent);
                    break;

            }
        }
    }
}
