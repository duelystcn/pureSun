
using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Hand
{
    public class HandGridItem
    { 
        public List<HandCellItem> handCells
        {
            get; private set;
        }
        public Color defaultColor = Color.blue;
        //模拟创建
        public void Create() {
            handCells = new List<HandCellItem>();
        }
        public void CreateCell(CardEntry cardEntry) {
            HandCellItem handCellItem = new HandCellItem(cardEntry);
            handCellItem.color = defaultColor;
            handCellItem.uuid = System.Guid.NewGuid().ToString("N");
            handCells.Add(handCellItem);
        }
    }
}
