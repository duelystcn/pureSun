using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //生物属性
        public int atk { get; set; }
        public int def { get; set; }

    }
}
