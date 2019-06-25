namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardEntry
    {
        //类型
        public enum CardType { MinionCard, ResourceCard, TacticsCard };    
        public CardType WhichCard;
        public CardInfo cardInfo;
        //生物属性
        public string name { get; set; }
        public string type { get; set; }
        public int cost { get; set; }

        //生物属性
        public int atk { get; set; }
        public int def { get; set; }


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
            }
        }
    }
}
