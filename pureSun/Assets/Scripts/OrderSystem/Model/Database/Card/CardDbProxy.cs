﻿

using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Newtonsoft.Json;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
                Debug.LogError("the card [" + cardCode + "]is null");
            }
            return cardInfo;
        }
    }
}
