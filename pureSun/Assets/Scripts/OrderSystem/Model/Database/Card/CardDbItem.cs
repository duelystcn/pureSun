using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardDbItem
    {
        public Dictionary<string, CardInfo> cardInfoMap;

        public List<CardEntry> cardEntryPool = new List<CardEntry>();
       
    }
}
