namespace Assets.Scripts.OrderSystem.Event
{
    class MinionSystemEvent
    {
        //页面逻辑
        public const string MINION_VIEW = "MinionView";
        //生物区动画开始
        public const string MINION_VIEW_ANIMATION_START = "MinionViewRemoveOneCard";

        //生物的攻击发生了变化
        public const string MINION_VIEW_MINION_CHANGE_ATK = "MinionViewMinionChangeAtk";
        //生物的生命发生了变化
        public const string MINION_VIEW_MINION_CHANGE_DEF = "MinionViewMinionChangeDef";
        //生成了一个生物，添加到页面
        public const string MINION_VIEW_ADD_ONE_MINION = "MinionViewAddOneMinion";

        public const string MINION_VIEW_CHANGE_OVER = "MinionViewChangeOver";

        public const string MINION_VIEW_MINIONS_CHANGE = "MinionViewMinionsChange";

        //鼠标移入某个生物，判断是否需要展示出详细信息
        public const string MINION_VIEW_ON_POINTER_ENTER = "MinionViewOnPointerEnter";
        //鼠标移出某个生物，判断是否需要关闭详细信息
        public const string MINION_VIEW_ON_POINTER_EXIT = "MinionViewOnPointerExit";

        //生物进行了攻击
        public const string MINION_VIEW_ATTACK_TARGET_MINION = "MinionViewAttackTargetMinion";

        //生物进行了移动
        public const string MINION_VIEW_MOVE_TARGET_HEX_CELL = "MinionViewMoveTargetHexCell";
        //生物死亡
        public const string MINION_VIEW_ONE_MINION_IS_DEAD = "MinionViewOneMinionIsDead";



        //逻辑处理
        public const string MINION_SYS = "MinionSys";
        //效果渲染
        public const string MINION_SYS_EFFECT_HIGHLIGHT = "MinionSysEffectHighlight";

        public const string MINION_SYS_EFFECT_HIGHLIGHT_CLOSE = "MinionSysEffectHighlightClose";
        //生物死亡
        public const string MINION_SYS_ONE_MINION_IS_DEAD = "MinionSysOneMinionIsDead";



    }
}
