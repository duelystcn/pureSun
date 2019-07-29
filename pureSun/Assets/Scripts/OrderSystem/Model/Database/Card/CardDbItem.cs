using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardDbItem
    {
        public Dictionary<string, CardInfo> cardInfoMap;

        public List<CardEntry> cardEntryPool = new List<CardEntry>();


        //当前获取到第几组了
        public int ListPoolIndex = 0;
        //新牌池思路？
        //五张一组
        public List<List<CardEntry>> cardEntryListPool;
       
    }
}
