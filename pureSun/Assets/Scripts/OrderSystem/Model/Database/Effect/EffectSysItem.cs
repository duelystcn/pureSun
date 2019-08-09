﻿

using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.SpecialOperate.ChooseOperate;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetChoose;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetMinion;
using static Assets.Scripts.OrderSystem.Model.Database.Effect.EffectAction.EATargetPlayer;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect
{
    public class EffectSysItem
    {
        public Dictionary<string, EffectInfo> effectInfoMap;

        public void EffectActionReady(EffectInfo effect) {
            if (effect.target == "ONE_MINION") {
                effect.TargetMinionOne = (MinionCellItem minionCellItem) =>
                {
                    for (int n = 0; n < effect.impactTargets.Length; n++) {
                        ChangeMinion(effect.impactTargets[n], effect.impactContents[n], minionCellItem);
                    }
                };
            }
            else if (effect.target == "CHOOSE_ONE") {
                effect.TargetChooseGrid = (ChooseGridItem chooseGridItem) =>
                {
                    CardEntry[] cardEntrys = new CardEntry[effect.chooseEffectList.Length];
                    for (int n = 0; n < effect.chooseEffectList.Length; n++) {
                        CardEntry cardEntry = new CardEntry();
                    }
                };

            }
            else if (effect.target == "Player") {
                effect.TargetPlayerList = (List<PlayerItem> playerItemList) =>
                {
                    for (int n = 0; n < effect.TargetPlayerItems.Count; n++)
                    {
                        for (int m = 0; m < effect.impactTargets.Length; m++) {
                            ChangePlayer(effect.impactTargets[n], effect.impactContents[n], effect.TargetPlayerItems[n]);
                        }
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
        void ChangePlayer(string impactTarget, string impactContent, PlayerItem playerItem)
        {
            switch (impactTarget)
            {
                case "Hand":
                    playerItem.DrawCard(Convert.ToInt32(impactContent));
                    break;
                case "ManaUpperLimit":
                    playerItem.ChangeManaUpperLimit(Convert.ToInt32(impactContent));
                    break;
               

            }

           

        }
    }
}
