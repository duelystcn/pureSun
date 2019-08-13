

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public enum TraitType { supremcy, innovation, polymorphism ,error};
    public class TraitCombination
    {
        public List<TraitType> traitTypes = new List<TraitType>();
        public TraitType AddTraitType(string traitName)
        {
            TraitType traitType = getTraitTypeByString(traitName);
            traitTypes.Add(traitType);
            return traitType;

        }
        public TraitType getTraitTypeByString(string traitName) {
            TraitType traitType = TraitType.error;
            if (traitName == "supremcy") {
                traitType = TraitType.supremcy;
            }
            else if (traitName == "innovation")
            {
                traitType = TraitType.innovation;
            }
            else if (traitName == "polymorphism")
            {
                traitType = TraitType.polymorphism;
            }
            if (traitType == TraitType.error) {
                UtilityLog.Log("找不到对应的科技类型");
            }
            return traitType;

        }
    }
    
}
