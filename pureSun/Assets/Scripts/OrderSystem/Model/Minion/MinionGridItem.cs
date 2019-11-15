
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Minion
{
    public class MinionGridItem
    {
        public Dictionary<HexCoordinates, CardEntry> minionCells {
            get;private set;
        }
        public void Create() {
            minionCells = new Dictionary<HexCoordinates, CardEntry>();

        }
       
    }
}
