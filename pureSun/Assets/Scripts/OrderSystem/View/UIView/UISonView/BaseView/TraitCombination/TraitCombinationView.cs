


using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView.TraitSignComponent;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.TraitCombination
{
    public class TraitCombinationView : ViewBaseView
    {
        public TraitSignRowList TraitSignRowListMyself;
        public TraitSignRowList TraitSignRowListEnemy;



        public void UITraitCombinationSysInit(List<TraitType> traitTypes, bool myself) {
            if (myself)
            {
                TraitSignRowListMyself.UITraitTypeInit(traitTypes);
            }
            else {
                TraitSignRowListEnemy.UITraitTypeInit(traitTypes);
            }

        }
        public void UITraitCombinationSysAdd(string traitType, bool myself) {
            if (myself)
            {
                TraitSignRowListMyself.UITraitTypeAdd(traitType);
            }
            else
            {
                TraitSignRowListEnemy.UITraitTypeAdd(traitType);
            }
        }


    }
}
