using Assets.Scripts.OrderSystem.Metrics;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardEntry
    {
        //类型
        public enum CardType { MinionCard, ResourceCard, TacticsCard, ShipCard };    
        public CardType WhichCard;
        public CardInfo cardInfo;
        //生物属性
        public string name { get; set; }
        public string type { get; set; }
        public int cost { get; set; }

        //生物属性
        public int atk { get; set; }
        public int def { get; set; }

        //卡牌背景
        public string bgImageName { get; set; }


        public void InitializeByCardInfo(CardInfo cardInfo) {
            this.cardInfo = cardInfo;
            this.name = cardInfo.name;
            this.type = cardInfo.type;
            this.cost = cardInfo.cost;
            switch (cardInfo.type)
            {
                case "Minion":
                    this.WhichCard = CardType.MinionCard;
                    this.atk = cardInfo.atk;
                    this.def = cardInfo.def;
                    break;
                case "Resource":
                    this.WhichCard = CardType.ResourceCard;
                    break;
                case "Tactics":
                    this.WhichCard = CardType.TacticsCard;
                    break;
                case "Ship":
                    this.WhichCard = CardType.ShipCard;
                    break;
            };
            int traitSum = 0;
            foreach (string trait in cardInfo.trait) {
                if (CardMetrics.CARD_TRAIT_I.Equals(trait)) {
                    traitSum += CardMetrics.CARD_TRAIT_I_NUM;
                } else if (CardMetrics.CARD_TRAIT_S.Equals(trait)) {
                    traitSum += CardMetrics.CARD_TRAIT_S_NUM;
                }
            }
            if (traitSum == CardMetrics.CARD_TRAIT_I_NUM)
            {
                this.bgImageName = CardMetrics.BGI_I;
            }
            else if (traitSum == CardMetrics.CARD_TRAIT_S_NUM)
            {
                this.bgImageName = CardMetrics.BGI_S;
            }
            else if (traitSum == CardMetrics.CARD_TRAIT_I_S_NUM)
            {
                this.bgImageName = CardMetrics.BGI_IS;
            }
        }
    }
}
