namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardInfo
    {
        //通用属性
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int cost { get; set; }
        public string effectName { get; set; }
        public string description { get; set; }

        //生物属性
        public int atk { get; set; }
        public int def { get; set; }

        //船只属性
        public string[] trait { get; set; }

        //限制数量
        public int quantity { get; set; }

    }
}
