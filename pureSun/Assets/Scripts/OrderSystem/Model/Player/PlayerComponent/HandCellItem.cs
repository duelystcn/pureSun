﻿

using Assets.Scripts.OrderSystem.Model.Database.Card;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public class HandCellItem
    {
        //顺序标识
        public int index;
        //卡的实例
        public CardEntry cardEntry { get; private set; }
        //颜色
        public Color color;
        //uuid做唯一标识
        public string uuid;



        public HandCellItem(CardEntry cardEntry)
        {
            this.cardEntry = cardEntry;
        }
    }
}