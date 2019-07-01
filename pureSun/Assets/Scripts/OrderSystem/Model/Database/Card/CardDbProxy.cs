

using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Newtonsoft.Json;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using System.IO;

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
        }
        //读取JSON文件配置
        public void LoadCardDbByJson() {
            string jsonStr = File.ReadAllText("Assets/Resources/Json/CardDb.json");
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
    }
}
