
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using System.Collections.Generic;
using UnityEngine.Events;
using static Assets.Scripts.OrderSystem.Model.Database.Card.CardEntry;
using static Assets.Scripts.OrderSystem.Model.Player.PlayerComponent.PlayerTimeTrigger;

namespace Assets.Scripts.OrderSystem.Model.Player
{
    public enum PlayerType { HumanPlayer, AIPlayer, NetPlayer };
    public class PlayerItem
    {

        //主要ID
        public string playerCode
        {
            get;  set;
        }
        public PlayerType playerType;
        //手牌
        //public HandGridItem handGridItem;

        //船
        public CardEntry shipCard;


        //牌组
        //public CardDeck cardDeck ;

        //墓地
        //public CardDeck cardGraveyard = new CardDeck();


        //起始点，虚拟坐标，用于确认召唤范围？
        public HexCoordinates hexCoordinates;

        //费用等可变属性保存
        public VariableAttributeMap playerVariableAttributeMap = new VariableAttributeMap();

        //科技相关TraitCombination
        public TraitCombination traitCombination;

        //分数？生命
        public int score = 0;

        //游戏模式中的玩家设定
        public GM_PlayerSite playerSiteOne;

        //添加在玩家身上的限制
        //每回合可以使用的资源牌的最大数
        public int canUseResourceMaxNumOneTurn = 1;
        //每回合可以使用的资源牌(每次使用会减少一次)
        public int canUseResourceNum = 0;
        //可使用资源次数恢复到最大数
        public void RestoreCanUseResourceNumMax() {
            canUseResourceNum = canUseResourceMaxNumOneTurn;
            ttPlayerHandCanUseJudge();
        }


        public PlayerItem(string playCode)
        {
            this.playerCode = playCode;
            playerVariableAttributeMap.CreateVariableAttributeByOriginalValueAndCodeAndBetterAndAutoRestore("Mana", 0, true);
            traitCombination = new TraitCombination();
        }

        //时点触发器
        //抽一张牌
        public TTPlayerNeedDrawACard ttPlayerNeedDrawACard;
        //获得了一张牌
        public TTPlayerGetACard ttPlayerGetACard;

        //移除一张牌
       // public TTPlayerRemoveACard ttPlayerRemoveACard;
        //使用一张牌
        public TTPlayerUseACard ttPlayerUseACard;
        //判断手牌是否可用
        public TTPlayerHandCanUseJudge ttPlayerHandCanUseJudge;

        //费用发生了变化
        public TTManaCostChange ttManaCostChange;



        //玩家分数发生了变化
        public TTScoreChange ttScoreChange;

        //增加了科技
        public TTAddTraitType ttAddTraitType;

        //固定可召唤区域
        public List<HexCoordinates> fixedCanCallHexList = new List<HexCoordinates>();
        //变动可召唤区域

        //禁止召唤区域


        //固定可移动区域
        public List<HexCoordinates> fixedCanMoveHexList = new List<HexCoordinates>();

        //制造可移动区域
        public void CreateCanMoveHex(GM_PlayerSite playerSiteOne)
        {
            foreach (string canCallRegionCode in playerSiteOne.canMoveRegionCodes)
            {
                foreach (GM_CellRegion gmCellRegion in playerSiteOne.cellRegionList)
                {
                    if (gmCellRegion.code == canCallRegionCode)
                    {
                        foreach (GM_CellCoordinate gMCell in gmCellRegion.regionCellList)
                        {
                            fixedCanMoveHexList.Add(new HexCoordinates(gMCell.x, gMCell.z));
                        }
                    }
                }
            }
        }
        //判断一个格子是否在移动范围内
        public bool CheckOneHexCanMove(HexCoordinates hexCoordinates) {
            foreach (HexCoordinates canMoveHex in fixedCanMoveHexList) {
                if (canMoveHex.X == hexCoordinates.X && canMoveHex.Y == hexCoordinates.Y) {
                    return true;
                }
            }
            return false;
        }


        //制造可召唤区域列表
        public void CreateCanCallHex(GM_PlayerSite playerSiteOne) {
            foreach (string canCallRegionCode in playerSiteOne.canCallRegionCodes) {
                foreach (GM_CellRegion gmCellRegion in playerSiteOne.cellRegionList)
                {
                    if (gmCellRegion.code == canCallRegionCode) {
                        foreach (GM_CellCoordinate gMCell in gmCellRegion.regionCellList) {
                            fixedCanCallHexList.Add(new HexCoordinates(gMCell.x, gMCell.z));
                        }
                    }
                }
            }
          
        }
        //读取游戏模式设定中的玩家设定
        public void LoadingGameModelPlayerSet(GM_PlayerSite playerSiteOne) {
            this.playerSiteOne = playerSiteOne;
            CreateCanCallHex(playerSiteOne);
            CreateCanMoveHex(playerSiteOne);
        }

        //判断一个格子是否在可召唤区域内
        public bool CheckOneCellCanCall(HexCoordinates hexCoordinates) {
            bool canCall = false;
            foreach (HexCoordinates oneHexCoordinates in fixedCanCallHexList) {
                if (hexCoordinates.X == oneHexCoordinates.X && hexCoordinates.Z == oneHexCoordinates.Z) {
                    canCall = true;
                }
            }
            return canCall;
        }
       
        //传入一个卡牌类型，检查是否有这个类型的牌
        //public bool checkhasonecardtypecard(cardtype cardtype) {
        //    bool hascard = false;
        //    foreach (cardentry handcellitem in this.handgriditem.handcells)
        //    {
        //        if (handcellitem.whichcard == cardtype) {
        //            hascard = true;
        //            return hascard;
        //        }
        //    }
        //    return hascard;
        //}
       
        //判断玩家是否可以再使用资源卡
        public bool CheckResourceCardCanUse() {
            bool canUse = true;
            if (canUseResourceNum <= 0)
            {
                canUse = false;
            }
            return canUse;
        }


        //判断玩家是否能使用一张牌
        public bool CheckOneCardCanUse(CardEntry cardEntry, QuestStageCircuitItem questStageCircuitItem) {
            if (questStageCircuitItem.oneTurnStage.automatic == "Y") {
                return false;
            }

            bool canUse = true;
            //判断是否还可以再使用资源卡
            if (cardEntry.WhichCard == CardEntry.CardType.ResourceCard)
            {
                if (questStageCircuitItem.stageHavePlayerCode != questStageCircuitItem.turnHavePlayerCode)
                {
                    return false;
                }
                canUse = CheckResourceCardCanUse();
                return canUse;
            }
            //检查是不是生物或者资源
            if (cardEntry.WhichCard == CardEntry.CardType.MinionCard) {
                if (questStageCircuitItem.stageHavePlayerCode != questStageCircuitItem.turnHavePlayerCode)
                {
                    return false;
                }
            }

            //当前可用费用
            int manaUsable = this.playerVariableAttributeMap.GetValueByCodeAndType("Mana", VATtrtype.CalculatedValue);
            //当前科技
            List<TraitType> traitTypes = this.traitCombination.traitTypes;
              
            //检查费用
            if (cardEntry.cost > manaUsable)
            {
                canUse = false;
                return canUse;
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
                    return canUse;
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
                ttPlayerNeedDrawACard();
            }
        }
        //将一张牌放入墓地
        //public void addonecardtograveyard(cardentry cardentry) {
        //    utilitylog.log("放入墓地：" + cardentry.cardinfo.name,loguttype.operate);
        //    this.cardgraveyard.putonecard(cardentry);
        //}

        //因为使用而失去一张手牌
        public void RemoveOneCardByUse(CardEntry handCellItem)
        {
            ttPlayerUseACard(handCellItem);
            //RemoveOneCard(handCellItem);
        }
        //移除一张手牌
        //public void RemoveOneCard(cardentry handcellitem) {
        //    //找到目标要移除的牌
        //    cardentry targethand = null;
        //    foreach (cardentry handcell in handgriditem.handcells) {
        //        if (handcell.uuid == handcellitem.uuid) {
        //            targethand = handcell;
        //        }
        //    }
        //    if (targethand != null)
        //    {
        //        handgriditem.handcells.remove(targethand);
        //        ttplayerremoveacard(targethand);
        //    }
        //    else {
        //        utilitylog.logerror("找不到要移除的手牌");

        //    }
        //}
        //改变费用上限
        public void ChangeManaUpperLimit(int num)
        {
            playerVariableAttributeMap.ChangeValueByCodeAndType("Mana", VATtrtype.OriginalValue, num);
            playerVariableAttributeMap.ChangeValueByCodeAndType("Mana", VATtrtype.DamageValue, -num);
            ttManaCostChange(playerVariableAttributeMap.variableAttributeMap["Mana"]);
        }
        //改变可用费用
        public void ChangeManaUsable(int num)
        {
            playerVariableAttributeMap.ChangeValueByCodeAndType("Mana", VATtrtype.DamageValue, num);
            ttManaCostChange(playerVariableAttributeMap.variableAttributeMap["Mana"]);
        }
        //判断是否可以改变费用
        public bool CheckCanChangeManaUsable(int num)
        {
            if (playerVariableAttributeMap.GetValueByCodeAndType("Mana", VATtrtype.CalculatedValue) + num < 0)
            {
                return false;
            }
            else {
                return true;
            }
        }
        //费用恢复至上限
        public void RestoreToTheUpperLimit()
        {
            int changeNum = playerVariableAttributeMap.GetValueByCodeAndType("Mana", VATtrtype.OriginalValue) - playerVariableAttributeMap.GetValueByCodeAndType("Mana", VATtrtype.CalculatedValue);
            playerVariableAttributeMap.ResetChangeValueAndDamageValue("Mana");
            ttManaCostChange(playerVariableAttributeMap.variableAttributeMap["Mana"]);
        }
        //载入指定的费用上限和可用费用
        public void LoadingManaInfo(int originalValue, int canUseValue) {
            playerVariableAttributeMap.ChangeValueByCodeAndType("Mana", VATtrtype.OriginalValue, originalValue);
            playerVariableAttributeMap.ChangeValueByCodeAndType("Mana", VATtrtype.DamageValue, canUseValue - originalValue);
        }

        //改变分数
        public void ChangeSocre(int changeNum)
        {
            score += changeNum;
            ttScoreChange(changeNum);
        }
        //因为打出一张牌而减少了费用
        public void ChangeManaUsableByUseHand(CardEntry chooseHand) {
            ChangeManaUsable(-chooseHand.cost);
        }


    }


}
