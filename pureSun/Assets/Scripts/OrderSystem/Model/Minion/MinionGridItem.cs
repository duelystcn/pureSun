
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Minion
{
    public class MinionGridItem
    {
        public Dictionary<int, MinionCellItem> minionCells {
            get;private set;
        }
        public void Create() {
            minionCells = new Dictionary<int, MinionCellItem>();

        }
       
    }
}
