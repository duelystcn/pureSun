
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System.Collections.Generic;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.Model.Player.PlayerComponent.PlayerTimeTrigger;

namespace Assets.Scripts.OrderSystem.Model.Player
{
    public enum PlayerType { HumanPlayer, AIPlayer, NetPlayer };
    public class PlayerItem
    {

        //主要ID
        public string playerCode
        {
            get; private set;
        }
        public PlayerType playerType;
        //手牌
        public HandGridItem handGridItem;

        //船
        public CardEntry shipCard;


        //牌组
        public CardDeck cardDeck ;


        //起始点，虚拟坐标，用于确认召唤范围？
        public HexCoordinates hexCoordinates;

        //费用相关
        public ManaItem manaItem;

        //科技相关TraitCombination
        public TraitCombination traitCombination;

        //分数？生命
        public int score = 0;



        public PlayerItem(string playCode)
        {
            this.playerCode = playCode;
            handGridItem = new HandGridItem();
            handGridItem.Create();
            cardDeck = new CardDeck();
            manaItem = new ManaItem();
            traitCombination = new TraitCombination();
        }

        //时点触发器
        //抽一张牌
        public TTPlayerDrawACard ttPlayerDrawACard;
        //移除一张牌
        public TTPlayerRemoveACard ttPlayerRemoveACard;

        //费用上限发生了变化
        public TTManaCostLimitChange ttManaCostLimitChange;
        //可用费用发生了变化
        public TTManaCostUsableChange ttManaCostUsableChange;

        //玩家分数发生了变化
        public TTScoreChange ttScoreChange;

        //增加了科技
        public TTAddTraitType ttAddTraitType;

        //固定可召唤区域
        public List<HexCoordinates> fixedCanCellHexList = new List<HexCoordinates>();
        //变动可召唤区域

        //禁止召唤区域

        //制造可召唤区域列表
        public void CreateCanCallHex(HexModelInfo hexModelInfo, HexCellItem[] cells, bool isMyself) {
            if (hexModelInfo.hexModelType == HexModelType.Source) {
                if (isMyself)
                {
                    fixedCanCellHexList.Add(new HexCoordinates(0, 0));
                    fixedCanCellHexList.Add(new HexCoordinates(1, 0));
                    fixedCanCellHexList.Add(new HexCoordinates(2, -1));
                    fixedCanCellHexList.Add(new HexCoordinates(3, -1));
                    fixedCanCellHexList.Add(new HexCoordinates(4, -2));
                    fixedCanCellHexList.Add(new HexCoordinates(5, -2));
                }
                else {

                }
            }
        }
        //判断一个格子是否在可召唤区域内
        public bool checkOneCellCanCall(HexCoordinates hexCoordinates) {
            bool canCall = false;
            foreach (HexCoordinates oneHexCoordinates in fixedCanCellHexList) {
                if (hexCoordinates.X == oneHexCoordinates.X && hexCoordinates.Z == oneHexCoordinates.Z) {
                    canCall = true;
                }
            }
            return canCall;
        }

        //判断玩家是否能使用一张牌
        public bool checkOneCardCanUse(CardEntry cardEntry) {
            bool canUse = true;
            //当前可用费用
            int manaUsable = this.manaItem.manaUsable;
            //当前科技
            List<TraitType> traitTypes = this.traitCombination.traitTypes;
              
            //检查费用
            if (cardEntry.cost > manaUsable)
            {
                canUse = false;
            }
            //检查科技要求
            HashSet<string> traitdemandSet = new HashSet<string>(cardEntry.traitdemand);
            foreach (string traitdemandNeed in traitdemandSet)
            {
                //需求是多少个
                int traitdemandNum = 0;
                foreach (string traitdemand in cardEntry.traitdemand)
                {
                    if (traitdemand == traitdemandNeed)
                    {
                        traitdemandNum++;
                    }
                }
                //目前的科技有多少
                int traitTypeNum = 0;
                foreach (TraitType traitType in traitTypes)
                {
                    if (traitType.ToString() == traitdemandNeed)
                    {
                        traitTypeNum++;
                    }
                }
                if (traitdemandNum > traitTypeNum)
                {
                    canUse = false;
                }
            }
            return canUse;
        }


        //增加了一点科技
        public void AddTraitType(string traitName)
        {
            TraitType traitType = traitCombination.AddTraitType(traitName);
            ttAddTraitType(traitType);

        }


        //方法抽一张牌
        public void DrawCard(int num) {
        for (int n = 0; n < num; n++) {
            HandCellItem handcellItem = this.handGridItem.CreateCell(this.cardDeck.GetFirstCard());
            ttPlayerDrawACard(handcellItem);
        }
        }
        //移除一张手牌
        public void RemoveOneCard(HandCellItem handCellItem) {
            //找到目标要移除的牌
            HandCellItem targetHand = null;
            foreach (HandCellItem handCell in handGridItem.handCells) {
                if (handCell.cardEntry.uuid == handCellItem.cardEntry.uuid) {
                    targetHand = handCell;
                }
            }
            if (targetHand != null)
            {
                handGridItem.handCells.Remove(targetHand);
                ttPlayerRemoveACard(targetHand);
            }
            else {
                UtilityLog.LogError("找不到要移除的手牌");

            }
           
        }
        //改变费用上限
        public void ChangeManaUpperLimit(int num)
        {
            manaItem.changeManaUpperLimit(num);
            ttManaCostLimitChange(num);
        }
        //改变可用费用
        public void ChangeManaUsable(int num)
        {
            manaItem.changeManaUsable(num);
            ttManaCostUsableChange(num);
        }
        //费用恢复至上限
        public void RestoreToTheUpperLimit()
        {
            int changeNum = manaItem.manaUpperLimit - manaItem.manaUsable;
            manaItem.RestoreToTheUpperLimit();
            ttManaCostUsableChange(changeNum);
        }
        //改变分数
        public void ChangeSocre(int changeNum)
        {
            score += changeNum;
            ttScoreChange(changeNum);
        }


    }


}
