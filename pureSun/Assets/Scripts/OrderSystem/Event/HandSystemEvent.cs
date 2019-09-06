namespace Assets.Scripts.OrderSystem.Event
{
    class HandSystemEvent
    {

        /// <summary>
        /// 手牌相关
        /// </summary>
        public const string HAND_CHANGE = "handChange";
        //某一玩家手牌注入
        public const string HAND_CHANGE_AFFLUX = "handChangeAfflux";
        //手牌变动完毕
        public const string HAND_CHANGE_OVER = "handChangeOver";
        //鼠标移动到某一手牌上
        public const string HAND_CHANGE_POINTER_ENTER = "handChangePointerEnter";
        //鼠标移动到某一手牌上
        public const string HAND_CHANGE_POINTER_EXIT = "handChangePointerExit";
        //抽了一张牌
        public const string HAND_CHANGE_DRAW_ONE_CARD = "handChangeDrawOneCard";
        //失去一张牌
        public const string HAND_CHANGE_REMOVE_ONE_CARD = "handChangeRemoveOneCard";
        //手牌区动画开始
        public const string HAND_CHANGE_ANIMATION_START = "handChangeRemoveOneCard";
        //手牌可用渲染变化
        public const string HAND_CHANGE_CAN_USE_JUDGE = "handChangeCanUseJudge";
        //手牌恢复到初始状态，当使用手牌没有成功的
        public const string HAND_CHANGE_UNCHECK_STATUS = "handChangeUncheckStatus";


        //手牌操作
        public const string HAND_OPERATE = "handOperate";
        //手牌选中
        public const string HAND_OPERATE_ONCHOOSE = "handOperateOnChoose";
    }
}
