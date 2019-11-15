

namespace Assets.Scripts.OrderSystem.Model.Database.Card.CardEntryComponent
{
    public class CardEntryTimeTrigger
    {
        //卡牌需要变更归属地
        public delegate void TTNeedChangeGameContainerType(CardEntry cardEntry);

        //卡牌变更归属地
        public delegate void TTCardChangeGameContainerType(CardEntry cardEntry);

        //卡牌被确定使用
        public delegate void TTCardNeedHideInView(CardEntry cardEntry);

        //卡牌需要添加监听信息到监听系统中
        public delegate void TTCardNeedAddToTTS(CardEntry cardEntry);

        //生物数值发生了变化
        public delegate void TTCardAttributeChange();
        //生物受到伤害


        //生物的buff发生了变化
        public delegate void TTCardBuffChange();

        //生物发起一次攻击,，不是时点，是通知效果执行器来发起攻击
        public delegate void TTCardLaunchAnAttack();

        //生物进行一次攻击
        public delegate void TTCardExecuteAnAttack();

        //生物的buff需要被移除
        public delegate void TTCardBuffNeedRemove();

        //生物发起一次移动
        public delegate void TTCardLaunchAnMove();

        //生物进行一次移动
        public delegate void TTCardExecuteAnMove();

        //生物死亡
        public delegate void TTCardMinionIsDead();

        //生物进入战场
        public delegate void TTCardMinionIntoBattlefield();

        //生物被牺牲
        public delegate void TTCardMinionToSacrifice();

        //
    }
}
