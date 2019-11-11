
using Assets.Scripts.OrderSystem.Model.Common.BasicGame;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Player.PlayerComponent
{
    public class CardDeck_Deprecated : BasicGameListDto
    {
        public List<CardEntry> cardEntryList = new List<CardEntry>();
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
        public void PutOneCard(CardEntry card)
        {
            cardEntryList.Add(card);

        }
    }
}
