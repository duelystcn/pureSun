
using Assets.Scripts.OrderSystem.Model.Database.Card;
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
        public void AddOneMinion(int index,CardEntry cardEntry) {
            MinionCellItem minionCellItem = new MinionCellItem();
            minionCellItem.cardEntry = cardEntry;
            minionCellItem.color = Color.red;
            minionCellItem.IsHighLight = false;
            minionCellItem.uuid = System.Guid.NewGuid().ToString("N");
            minionCells.Add(index,minionCellItem);
        }
    }
}
