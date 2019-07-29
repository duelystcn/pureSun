

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public class HandGridItem
    {
        public List<HandCellItem> handCells
        {
            get; private set;
        }
        public Color defaultColor = Color.blue;
        //模拟创建
        public void Create()
        {
            handCells = new List<HandCellItem>();
        }
        public void CreateCell(CardEntry cardEntry)
        {
            HandCellItem handCellItem = new HandCellItem(cardEntry);
            handCellItem.color = defaultColor;
            handCellItem.uuid = System.Guid.NewGuid().ToString("N");
            handCells.Add(handCellItem);
        }

        //移除一张手牌的实例
        public void RemoveOneHandCellItem(HandCellItem handCellItem)
        {
            int index = -1;
            for (int i = 0; i < this.handCells.Count; i++)
            {
                if (this.handCells[i].X == handCellItem.X)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                UtilityLog.LogError("This handCellItem index" + handCellItem.X + "is not exist");
            }
            else
            {
                this.handCells.RemoveAt(index);
            }
        }
    }
}
