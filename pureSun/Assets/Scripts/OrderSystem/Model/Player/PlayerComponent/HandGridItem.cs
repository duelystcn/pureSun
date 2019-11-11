

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Common.BasicGame;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    //这个类弃用了
    public class HandGridItem_Deprecated : BasicGameListDto
    {
        public List<CardEntry> handCells
        {
            get; private set;
        }
        public Color defaultColor = Color.blue;

        //index判断哪一张是最新加的？
        private int cellIndex;

        public string playerCode;

        //模拟创建
        public void Create()
        {
            handCells = new List<CardEntry>();
            cellIndex = 0;
        }
      
        public CardEntry CreateCell(CardEntry cardEntry)
        {
            cardEntry.locationIndex = cellIndex;
            cellIndex++;
            handCells.Add(cardEntry);
            return cardEntry;
        }

        //移除一张手牌的实例
        public void RemoveOneHandCellItem(CardEntry cardEntry)
        {
            int index = -1;
            for (int i = 0; i < this.handCells.Count; i++)
            {
                if (this.handCells[i].uuid == cardEntry.uuid)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                UtilityLog.LogError("This handCellItem index" + cardEntry.locationIndex + "is not exist");
            }
            else
            {
                this.handCells.RemoveAt(index);
            }
        }
    }
}
