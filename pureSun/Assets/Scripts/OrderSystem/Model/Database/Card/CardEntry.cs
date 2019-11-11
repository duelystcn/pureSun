using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Common.BasicGame;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.ChooseMakeStage;
using static Assets.Scripts.OrderSystem.Model.Database.Card.CardEntryComponent.CardEntryTimeTrigger;

namespace Assets.Scripts.OrderSystem.Model.Database.Card
{
    public class CardEntry : BasicGameDto
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

        public string uuid { get; set; }

        public string[] traitdemand { get; set; }
        //是否瞬间
        public string isMoment { get; set; }

        //效果
        public string[] effectCodeList { get; set; }
        //描述
        public string description { get; set; }

        //上一个归属地
        public string lastGameContainerType;
        //归属地
        public string gameContainerType;
        //下一个归属地
        public string nextGameContainerType;

        //在归属地中的坐标
        public int locationIndex;



        //判断是否可使用标识
        public bool canUse = false;




        //功能属性
        //复合式选择框所属层级
        public VCSLayerSort layerSort;
        //是否已被购买
        public bool isBuyed = false;




        //已被触发的效果
        public EffectInfo triggeredEffectInfo;

        

        //需要被展示的效果
        public EffectInfo needShowEffectInfo;


        //目标暂存
        public BasicGameDto targetBasicGameDto;


        //卡牌需要变更归属地
        public TTNeedChangeGameContainerType ttNeedChangeGameContainerType;

        //根据效果实例化一张卡（用作展示）
        public void InitializeByEffectInfo(EffectInfo oneEffectInfo)
        {
            this.effectCodeList = new string[1] { oneEffectInfo.code };
            this.description = oneEffectInfo.description;
        }

            //根据cardInfo实例化一张卡
        public void InitializeByCardInfo(CardInfo cardInfo) {
            this.cardInfo = cardInfo;
            this.name = cardInfo.name;
            this.type = cardInfo.type;
            this.cost = cardInfo.cost;
            this.description = cardInfo.description;
            this.traitdemand = cardInfo.traitdemand;
            this.uuid = System.Guid.NewGuid().ToString("N");
            this.isMoment = cardInfo.isMoment;
            this.effectCodeList = cardInfo.effectCodeList;
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
                }
                else if (CardMetrics.CARD_TRAIT_S.Equals(trait)) {
                    traitSum += CardMetrics.CARD_TRAIT_S_NUM;
                }
                else if (CardMetrics.CARD_TRAIT_N.Equals(trait))
                {
                    traitSum += CardMetrics.CARD_TRAIT_N_NUM;
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
            else if(traitSum == CardMetrics.CARD_TRAIT_N_NUM)
            {
                this.bgImageName = CardMetrics.BGI_N;
            }
        }
    }
}
