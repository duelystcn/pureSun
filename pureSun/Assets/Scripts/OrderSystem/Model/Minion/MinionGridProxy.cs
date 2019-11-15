using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Minion
{
    public class MinionGridProxy_Deprecated : Proxy
    {
        public new const string NAME = "MinionGridProxy";

        public MinionGridItem minionGridItem
        {
            get { return (MinionGridItem)base.Data; }
        }

        public MinionGridProxy_Deprecated() : base(NAME)
        {
            MinionGridItem minionGridItem = new MinionGridItem();
            minionGridItem.Create();
            base.Data = minionGridItem;
        }
        public CardEntry GetMinionCellItemByIndex(HexCoordinates index) {
            if (minionGridItem.minionCells.ContainsKey(index))
            {
                return minionGridItem.minionCells[index];
            }
            else {
                return null;
            }
        }
        
       
        
        //根据玩家code获取该玩家的所有生物
        public List<CardEntry> GetMinionCellItemListByPlayerCode(PlayerItem playerItem) {
            List<CardEntry> minionCellItems = new List<CardEntry>();
            Dictionary<HexCoordinates, CardEntry>.KeyCollection keyCol = minionGridItem.minionCells.Keys;
            foreach (HexCoordinates key in keyCol)
            {
                CardEntry minionCellItem = minionGridItem.minionCells[key];
                if (minionCellItem.controllerPlayerItem.playerCode == playerItem.playerCode) {
                    minionCellItems.Add(minionCellItem);
                }
            }
            return minionCellItems;
        }
        //获取所有的生物
        public List<CardEntry> GetMinionCellItemListAll()
        {
            List<CardEntry> minionCellItems = new List<CardEntry>();
            Dictionary<HexCoordinates, CardEntry>.KeyCollection keyCol = minionGridItem.minionCells.Keys;
            foreach (HexCoordinates key in keyCol)
            {
                minionCellItems.Add(minionGridItem.minionCells[key]);
            }
            return minionCellItems;
        }

      

    }
}
