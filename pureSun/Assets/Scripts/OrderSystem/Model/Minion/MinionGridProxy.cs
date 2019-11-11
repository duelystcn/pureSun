﻿using Assets.Scripts.OrderSystem.Common.UnityExpand;
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
        public MinionCellItem GetMinionCellItemByIndex(HexCoordinates index) {
            if (minionGridItem.minionCells.ContainsKey(index))
            {
                return minionGridItem.minionCells[index];
            }
            else {
                return null;
            }
        }
        //根据手牌添加生物
        public void AddOneMinionByHand(HexCoordinates index, CardEntry chooseHand)
        {
            AddOneMinionByCard(index,chooseHand);
        }
        //根据指定牌添加生物
        public void AddOneMinionByCard(HexCoordinates index, CardEntry cardEntry)
        {
            //判断这个坐标上是否已经有生物存在
            if (minionGridItem.minionCells.ContainsKey(index)) {
                UtilityLog.LogError("该位置已存在生物");
                return;
            }
            MinionCellItem minionCellItem = new MinionCellItem();
            minionCellItem.cardEntry = cardEntry;
            minionCellItem.dtoType = "Minion";
            //设置所有者
            minionCellItem.controllerPlayerItem = cardEntry.controllerPlayerItem;
            minionCellItem.color = Color.red;
            minionCellItem.IsEffectTarget = false;
            minionCellItem.uuid = System.Guid.NewGuid().ToString("N");
            minionCellItem.index = index;
            minionCellItem.minionVariableAttributeMap.CreateVariableAttributeByOriginalValueAndCodeAndBetterAndAutoRestore("Atk", cardEntry.atk, true);
            minionCellItem.minionVariableAttributeMap.CreateVariableAttributeByOriginalValueAndCodeAndBetterAndAutoRestore("Def", cardEntry.def, true);
            minionCellItem.minionComeFrom = MinionComeFrom.Hand;
            AddTimeTrigger(minionCellItem);
            minionGridItem.minionCells.Add(index, minionCellItem);
            minionCellItem.ttMinionIntoBattlefield();
            SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_ADD_ONE_MINION);
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
            //buff发生了变化
            minionCellItem.ttBuffChange = () =>
            {
                SendNotification(UIViewSystemEvent.UI_ONE_CARD_ALL_INFO, minionCellItem, UIViewSystemEvent.UI_ONE_CARD_ALL_INFO_BUFF_CHANGE);
            };
            //buff需要被移除
            minionCellItem.ttBuffNeedRemove = () =>
            {
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_MINION_BUFF_NEED_REMOVE);
            };
            //生物准备发起一次攻击
            minionCellItem.ttLaunchAnAttack = () =>{
                UtilityLog.Log("玩家【" + minionCellItem .controllerPlayerItem.playerCode + "】的生物【" + minionCellItem.cardEntry.name + "】发起一次攻击", LogUtType.Attack);
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_LAUNCH_AN_ATTACK);
            };
            //生物进行一次攻击
            minionCellItem.ttExecuteAnAttack = () =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_ATTACK_TARGET_MINION);
            };
            //生物发起一次移动
            minionCellItem.ttLaunchAnMove = () =>
            {
                UtilityLog.Log("玩家【" + minionCellItem.controllerPlayerItem.playerCode + "】的生物【" + minionCellItem.cardEntry.name + "】发起一次移动", LogUtType.Attack);
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_LAUNCH_AN_MOVE);
            };
            //生物进行一次移动
            minionCellItem.ttExecuteAnMove = () =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_MOVE_TARGET_HEX_CELL);
            };
            //生物死亡
            minionCellItem.ttMinionIsDead = () =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_ONE_MINION_IS_DEAD);
                SendNotification(MinionSystemEvent.MINION_SYS, minionCellItem, MinionSystemEvent.MINION_SYS_ONE_MINION_IS_DEAD);
            };
            //生物进入战场
            minionCellItem.ttMinionIntoBattlefield = () => {
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_MINION_ENTER_THE_BATTLEFIELD);

            };
        }
        //根据玩家code获取该玩家的所有生物
        public List<MinionCellItem> GetMinionCellItemListByPlayerCode(PlayerItem playerItem) {
            List<MinionCellItem> minionCellItems = new List<MinionCellItem>();
            Dictionary<HexCoordinates, MinionCellItem>.KeyCollection keyCol = minionGridItem.minionCells.Keys;
            foreach (HexCoordinates key in keyCol)
            {
                MinionCellItem minionCellItem = minionGridItem.minionCells[key];
                if (minionCellItem.cardEntry.controllerPlayerItem.playerCode == playerItem.playerCode) {
                    minionCellItems.Add(minionCellItem);
                }
            }
            return minionCellItems;
        }

        //生物移动到指定地点
        public void MoveToTargetHexCoordinates(MinionCellItem minionCellItem, HexCoordinates targetMoveHexCoordinates) { 
            minionCellItem.lastIndex = minionCellItem.index;
            minionCellItem.index = targetMoveHexCoordinates;
            minionGridItem.minionCells.Add(targetMoveHexCoordinates, minionCellItem);
            minionGridItem.minionCells.Remove(minionCellItem.lastIndex);
            minionCellItem.ttExecuteAnMove();
        }
        

    }
}
