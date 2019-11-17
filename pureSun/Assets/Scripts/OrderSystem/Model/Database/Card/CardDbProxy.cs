

using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
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
        //根据code获取一张卡的实例
        public CardEntry GetCardEntryByCode(string cardCode)
        {
            CardEntry cardEntry = new CardEntry();
            cardEntry.InitializeByCardInfo(this.GetCardInfoByCode(cardCode));
            AddTimeTriggerForCardEntry(cardEntry);
            return cardEntry;
        }
        //根据cardInfo获取一张卡的实例
        public CardEntry GetCardEntryBCardInfo(CardInfo cardInfo)
        {
            CardEntry cardEntry = new CardEntry();
            cardEntry.InitializeByCardInfo(cardInfo);
            AddTimeTriggerForCardEntry(cardEntry);
            return cardEntry;
        }
        //给一张卡绑定上时点关系
        public void AddTimeTriggerForCardEntry(CardEntry addTTcardEntry)
        {
            addTTcardEntry.ttNeedChangeGameContainerType = (CardEntry cardEntry) => {
                SendNotification(GameContainerEvent.GAME_CONTAINER_SYS, cardEntry, GameContainerEvent.GAME_CONTAINER_SYS_CARD_NEED_MOVE);
            };
            addTTcardEntry.ttCardChangeGameContainerType = (CardEntry cardEntry) => {
                SendNotification(UIViewSystemEvent.UI_CARD_ENTRY_SYS, cardEntry, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_CARD_ENTRY_SYS_CHANGE_GAME_CONTAINER_TYPE, cardEntry.controllerPlayerItem.playerCode));
                SendNotification(TimeTriggerEvent.TIME_TRIGGER_SYS, cardEntry, StringUtil.GetNTByNotificationTypeAndPlayerCode(TimeTriggerEvent.TIME_TRIGGER_SYS_CARD_CHANGE_GAME_CONTAINER_TYPE, cardEntry.controllerPlayerItem.playerCode));
                SendNotification(HexSystemEvent.HEX_VIEW_SYS, cardEntry, HexSystemEvent.HEX_VIEW_SYS_CARD_CHANGE_GAME_CONTAINER_TYPE);
            };
            addTTcardEntry.ttCardNeedHideInView = (CardEntry cardEntry) => {
                SendNotification(UIViewSystemEvent.UI_CARD_ENTRY_SYS, cardEntry, StringUtil.GetNTByNotificationTypeAndPlayerCode(UIViewSystemEvent.UI_CARD_ENTRY_SYS_CARD_NEED_HIDE_IN_VIEW, cardEntry.controllerPlayerItem.playerCode));
            };
            addTTcardEntry.ttCardNeedAddToTTS = (CardEntry cardEntry) =>
            {
                SendNotification(GameContainerEvent.GAME_CONTAINER_SYS, cardEntry, GameContainerEvent.GAME_CONTAINER_SYS_CARD_NEED_ADD_TO_TTS);
            };

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
                    CardEntry card = this.GetCardEntryBCardInfo(cardInfos[n]);
                    cardDbItem.cardEntryPool.Add(card);
                }
            }
            //创建牌池组
            CreateCardEntryListPool();
        }
        public void CreateCardEntryListPool() {
            cardDbItem.cardEntryListPool = new List<List<CardEntry>>();
            List<CardEntry> cardEntries = GetSameCardEntry(40);
            //5个一组
            int size = 5;
            int lnum = cardEntries.Count / size;
            for (int m = 0; m < lnum; m++) {
                List<CardEntry> oneList = new List<CardEntry>();
                cardDbItem.cardEntryListPool.Add(oneList);
            }

            for (int n = 0; n < cardEntries.Count; n++) {
                cardDbItem.cardEntryListPool[n % lnum].Add(cardEntries[n]);
            }

        }



        //获取N张随机牌池的牌
        public List<CardEntry> GetSameCardEntry(int num) {
            if (num > cardDbItem.cardEntryPool.Count) {
                UtilityLog.LogError("need card number too much");
                return null;
            }
            List<int> randomList = RandomUtil.GetRandom(0, cardDbItem.cardEntryPool.Count-1, num, false);
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

        //获取一组牌
        public List<CardEntry> GetOneCardListForPool() {
            List<CardEntry> cardEntries = cardDbItem.cardEntryListPool[cardDbItem.ListPoolIndex];
            cardDbItem.ListPoolIndex++;
            return cardEntries;
        }
    }
}
