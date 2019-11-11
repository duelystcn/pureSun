namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardInfo
    {
        //通用属性
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int cost { get; set; }


        public string isMoment { get; set; }
        public string[] effectCodeList { get; set; }
        public string description { get; set; }

        //生物属性
        public int atk { get; set; }
        public int def { get; set; }

        //卡面属性
        public string[] trait { get; set; }


        //等级属性
        public string[] traitdemand { get; set; }

        //限制数量
        public int quantity { get; set; }

        //是否需要指定对象
        public string[] targetSetToChooseList { get; set; }

    }
}
