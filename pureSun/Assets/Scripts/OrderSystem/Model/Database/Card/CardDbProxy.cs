

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Metrics;
using Newtonsoft.Json;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardDbProxy : Proxy
    {
        public new const string NAME = "CardDbProxy";
        public CardDbItem cardDbItem
        {
            get { return (CardDbItem)base.Data; }
        }
        
        public CardDbProxy() : base(NAME)
        {
            CardDbItem cardDbItem = new CardDbItem();
            base.Data = cardDbItem;
            LoadCardDbByJson();
            CreateCardEntryPool();
        }
        //读取JSON文件配置
        public void LoadCardDbByJson() {
            string jsonStr = File.ReadAllText("Assets/Resources/Json/CardDb.json", Encoding.GetEncoding("gb2312"));
            cardDbItem.cardInfoMap = 
                JsonConvert.DeserializeObject<Dictionary<string, CardInfo>>(jsonStr);
            jsonStr = File.ReadAllText("Assets/Resources/Json/EffectDb.json");

        }
        //根据code获取一张卡信息
        public CardInfo GetCardInfoByCode(string cardCode) {
            CardInfo cardInfo = cardDbItem.cardInfoMap[cardCode];
            if (cardInfo == null) {
                UtilityLog.LogError("the card [" + cardCode + "]is null");
            }
            return cardInfo;
        }
        //根据Type获取信息
        public List<CardInfo> GetCardInfoByType(string cardType) {
            List<CardInfo> cardInfos = new List<CardInfo>();
            foreach (CardInfo cardInfo in cardDbItem.cardInfoMap.Values) {
                if (cardInfo.type.Equals(cardType)) {
                    cardInfos.Add(cardInfo);
                }
            }
            return cardInfos;
        }
        //创建牌池
        public void CreateCardEntryPool() {
            List<CardInfo> cardInfos = new List<CardInfo>();
            foreach (CardInfo cardInfo in cardDbItem.cardInfoMap.Values)
            {
                if (!cardInfo.type.Equals(CardMetrics.CARD_TYPE_SHIP))
                {
                    cardInfos.Add(cardInfo);
                }
            }
            for (int n = 0; n < cardInfos.Count; n++) {
                int quantity = cardInfos[n].quantity;
                for (int m = 0; m < quantity; m++) {
                    CardEntry card = new CardEntry();
                    card.cardInfo = cardInfos[n];
                    card.InitializeByCardInfo(cardInfos[n]);
                    cardDbItem.cardEntryPool.Add(card);
                }
            }
        }
        //获取N张随机牌池的牌
        public List<CardEntry> GetSameCardEntry(int num) {
            List<int> randomList = RandomUtil.GetRandom(0, cardDbItem.cardEntryPool.Count-1, 3, false);
            List<CardEntry> cardEntries = new List<CardEntry>();
            foreach (int i in randomList) {
                cardEntries.Add(cardDbItem.cardEntryPool[i]);
            }
            return cardEntries;
        }
        //玩家选择完成后从卡牌池移除此牌
        public void RemoveOneCardEntry(CardEntry cardEntry) {
            cardDbItem.cardEntryPool.Remove(cardEntry);
        }
    }
}
