namespace Assets.Scripts.OrderSystem.Event
{
    class MinionSystemEvent
    {
        //页面逻辑
        public const string MINION_VIEW = "MinionView";
        //生物的攻击发生了变化
        public const string MINION_VIEW_MINION_CHANGE_ATK = "MinionViewMinionChangeAtk";
        //生物的生命发生了变化
        public const string MINION_VIEW_MINION_CHANGE_DEF = "MinionViewMinionChangeDef";
        //


        public const string MINION_VIEW_CHANGE_OVER = "MinionViewChangeOver";
        public const string MINION_VIEW_MINIONS_CHANGE = "MinionViewMinionsChange";



        //逻辑处理
        public const string MINION_SYS = "MinionSys";
        //效果渲染
        public const string MINION_SYS_EFFECT_HIGHLIGHT = "MinionSysEffectHighlight";

        public const string MINION_SYS_EFFECT_HIGHLIGHT_CLOSE = "MinionSysEffectHighlightClose";
    }
}
