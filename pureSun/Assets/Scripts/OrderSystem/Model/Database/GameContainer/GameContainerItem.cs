

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Common.BasicGame;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;
using static Assets.Scripts.OrderSystem.Model.Database.Card.CardEntry;

namespace Assets.Scripts.OrderSystem.Model.Database.GameContainer
{
   
    public class GameContainerItem : BasicGameDto
    {
        public string gameContainerType;

        public List<CardEntry> cardEntryList = new List<CardEntry>();
        //index判断哪一张是最新加的？
        private int cellIndex;

        //模拟创建
        public void Create()
        {
            cellIndex = 0;
            this.dtoType = "GameContainerItem";
        }

        //测试初始化，随机添加一些牌
        public void TestInitializeRandom(Dictionary<string, CardInfo> cardInfoMap)
        {

        }
        //获取第一张牌并移除
        //获取序号为0的？
        public CardEntry GetFirstCard()
        {
            CardEntry cardEntry = cardEntryList[0];
            cardEntryList.RemoveAt(0);
            return cardEntry;
        }
        //放置一张卡牌进卡组
        public CardEntry PutOneCard(CardEntry card)
        {
            card.locationIndex = cellIndex;
            cellIndex++;
            cardEntryList.Add(card);
            return card;
        }

        //移除一张手牌的实例
        public void RemoveOneCardEntry(CardEntry cardEntry)
        {
            int index = -1;
            for (int i = 0; i < this.cardEntryList.Count; i++)
            {
                if (this.cardEntryList[i].uuid == cardEntry.uuid)
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
                cardEntry.lastGameContainerType = this.gameContainerType;
                this.cardEntryList.RemoveAt(index);
            }
        }
        //手牌可使用判断
        public void ChangeHandCardCanUse(QuestStageCircuitItem questStageCircuitItem)
        {
            foreach (CardEntry handCellItem in this.cardEntryList)
            {
                handCellItem.canUse = handCellItem.controllerPlayerItem.CheckOneCardCanUse(handCellItem, questStageCircuitItem);
            }
        }

        //获取一张卡牌类型的牌，暂时先返回第一张
        public CardEntry GetOneCardTypeCard(CardType cardType)
        {
            CardEntry getHand = null;
            foreach (CardEntry cardEntry in this.cardEntryList)
            {
                if (cardEntry.WhichCard == cardType)
                {
                    return cardEntry;
                }
            }
            return getHand;
        }
        //获取一张卡牌类型的牌，暂时先返回第一张
        public CardEntry GetOneCardByCardCode(string cardCode)
        {
            CardEntry getHand = null;
            foreach (CardEntry cardEntry in this.cardEntryList)
            {
                if (cardEntry.cardInfo.code == cardCode)
                {
                    return cardEntry;
                }
            }
            return getHand;
        }


    }
}
