using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Model.Minion
{
    public class MinionGridProxy : Proxy
    {
        public new const string NAME = "MinionGridProxy";

        public MinionGridItem minionGridItem
        {
            get { return (MinionGridItem)base.Data; }
        }

        public MinionGridProxy() : base(NAME)
        {
            MinionGridItem minionGridItem = new MinionGridItem();
            minionGridItem.Create();
            base.Data = minionGridItem;
        }
        public MinionCellItem GetMinionCellItemByIndex(int index) {
            if (minionGridItem.minionCells.ContainsKey(index))
            {
                return minionGridItem.minionCells[index];
            }
            else {
                return null;
            }
        }
        //根据手牌添加生物
        public void AddOneMinionByHand(int index, HandCellItem chooseHand)
        {
            AddOneMinionByCard(index,chooseHand.cardEntry);
        }
        //根据指定牌添加生物
        public void AddOneMinionByCard(int index, CardEntry cardEntry)
        {
            MinionCellItem minionCellItem = new MinionCellItem();
            minionCellItem.cardEntry = cardEntry;
            minionCellItem.playerCode = cardEntry.player.playerCode;
            minionCellItem.color = Color.red;
            minionCellItem.IsEffectTarget = false;
            minionCellItem.uuid = System.Guid.NewGuid().ToString("N");
            AddTimeTrigger(minionCellItem);
            minionGridItem.minionCells.Add(index, minionCellItem);
        }
        //添加信号发射
        public void AddTimeTrigger(MinionCellItem minionCellItem) {
            minionCellItem.ttAtkChange = (int changeNum) =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_MINION_CHANGE_ATK);
            };
            minionCellItem.ttDefChange = (int changeNum) =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_MINION_CHANGE_DEF);
            };
        }
        //根据玩家code获取该玩家的所有生物
        public List<MinionCellItem> GetMinionCellItemListByPlayerCode(PlayerItem playerItem) {
            List<MinionCellItem> minionCellItems = new List<MinionCellItem>();
            Dictionary<int, MinionCellItem>.KeyCollection keyCol = minionGridItem.minionCells.Keys;
            foreach (int key in keyCol)
            {
                MinionCellItem minionCellItem = minionGridItem.minionCells[key];
                if (minionCellItem.cardEntry.player.playerCode == playerItem.playerCode) {
                    minionCellItems.Add(minionCellItem);
                }
            }
            return minionCellItems;
        }

    }
}
