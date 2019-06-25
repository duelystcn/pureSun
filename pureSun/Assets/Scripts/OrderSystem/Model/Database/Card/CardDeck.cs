using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardDeck
    {
        public List<CardEntry> cardEntryList;
        //测试初始化，随机添加一些牌
        public void TestInitializeRandom(Dictionary<string, CardInfo> cardInfoMap) {
           
        }
        //获取第一张牌并移除
        //获取序号为0的？
        public CardEntry GetFirstCard() {
            CardEntry cardEntry = cardEntryList[0];
            cardEntryList.RemoveAt(0);
            return cardEntry;
        }
    }
}
