using Assets.Scripts.OrderSystem.Metrics;
using Assets.Scripts.OrderSystem.Model.Common;
using Assets.Scripts.OrderSystem.Model.Common.BasicGame;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.ChooseMakeStage;
using System.Collections.Generic;
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
        //卡牌变更了归属地
        public TTCardChangeGameContainerType ttCardChangeGameContainerType;
        //卡牌确定被玩家使用
        public TTCardNeedHideInView ttCardNeedHideInView;
        //卡牌需要添加监听信息到监听系统中
        public TTCardNeedAddToTTS ttCardNeedAddToTTS;


        //时点触发器
        //生物数值发生了变化
        public TTCardAttributeChange ttAttributeChange;
        //生物Buff发生了变化,目前好像不会触发这方面的效果，主要是触发界面显示变化
        public TTCardBuffChange ttBuffChange;
        //生物Buff需要取消，将其发送到效果执行器进行取消
        public TTCardBuffNeedRemove ttBuffNeedRemove;
        //生物发起攻击
        public TTCardLaunchAnAttack ttLaunchAnAttack;
        //生物进行攻击
        public TTCardExecuteAnAttack ttExecuteAnAttack;

        //生物发起移动
        public TTCardLaunchAnMove ttLaunchAnMove;
        //生物进行移动
        public TTCardExecuteAnMove ttExecuteAnMove;

        //费用，攻击，生命等可变属性保存
        public VariableAttributeMap cardEntryVariableAttributeMap = new VariableAttributeMap();

        //生物进入战场
        public TTCardMinionIntoBattlefield ttMinionIntoBattlefield;

        public TTCardMinionToSacrifice ttMinionToSacrifice;

        public TTCardMinionIsDead ttCardMinionIsDead;

        //攻击的目标点，前端页面播放时需要用到
        public HexCoordinates attackTargetIndex;

        //所在位置
        // public int index;
        public HexCoordinates nowIndex;
        //原始所在位置，演算移动动画时需要用到
        public HexCoordinates lastIndex;

        //是否可作为效果对象
        public bool IsEffectTarget;

        //持续buff，放在一个list里
        public List<EffectInfo> effectBuffInfoList = new List<EffectInfo>();

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

        //生物受到伤害
        public void SufferDamage(int damageNum)
        {
            //cumulativeDamage += damageNum;

            cardEntryVariableAttributeMap.ChangeValueByCodeAndType("Def", VATtrtype.DamageValue, -damageNum);
            ttAttributeChange();
            CheckMinionIsDead();
        }
        //检查生物是否死亡
        public void CheckMinionIsDead()
        {
            if (cardEntryVariableAttributeMap.GetValueByCodeAndType("Def", VATtrtype.CalculatedValue) <= 0)
            {
                ttCardMinionIsDead();
            }

        }

        //生物攻击某一个生物
        public void AttackTargetMinion(CardEntry defensiveMinionCellItem)
        {
            this.attackTargetIndex = defensiveMinionCellItem.nowIndex;
            ttExecuteAnAttack();
            defensiveMinionCellItem.SufferDamage(cardEntryVariableAttributeMap.GetValueByCodeAndType("Atk", VATtrtype.CalculatedValue));
        }
        //生物反击某一个生物
        public void CounterAttackTargetMinion(CardEntry attackMinionCellItem)
        {
            this.attackTargetIndex = attackMinionCellItem.nowIndex;
            ttExecuteAnAttack();
            attackMinionCellItem.SufferDamage(cardEntryVariableAttributeMap.GetValueByCodeAndType("Atk", VATtrtype.CalculatedValue));
        }


        //检查是否需要清除buff
        public void CheckNeedChangeEffectBuffInfo(string timeTrigger)
        {
            bool buffHasChange = false;
            foreach (EffectInfo effectInfo in effectBuffInfoList)
            {
                //倒计时为0则清除buff
                if (effectInfo.effectiveTime.ContinuousStage == timeTrigger)
                {
                    effectInfo.effectiveTime.ContinuousRound--;
                    if (effectInfo.effectiveTime.ContinuousRound == 0)
                    {
                        buffHasChange = true;
                    }
                }

            }
            if (buffHasChange)
            {
                ttBuffNeedRemove();
                //清除失效buff
                List<EffectInfo> newEffectBuffInfoList = new List<EffectInfo>();
                foreach (EffectInfo effectInfo in effectBuffInfoList)
                {
                    if (effectInfo.effectiveTime.ContinuousRound > 0)
                    {
                        newEffectBuffInfoList.Add(effectInfo);
                    }
                }
                effectBuffInfoList = newEffectBuffInfoList;
                ttBuffChange();
            }
        }

        //生物移动到指定地点
        public void MoveToTargetHexCoordinates( HexCoordinates targetMoveHexCoordinates)
        {
            this.lastIndex = this.nowIndex;
            this.nowIndex = targetMoveHexCoordinates;
            this.ttExecuteAnMove();
        }


    }
}
