

namespace Assets.Scripts.OrderSystem.Event
{
    class UIViewSystemEvent
    {
        /// <summary>
        /// UI相关
        /// </summary>
        public const string UI_VIEW = "UIView";




        /// <summary>
        /// 开始选单相关
        /// </summary>
        public const string UI_START_MAIN = "UIStartMain";
        /// <summary>
        /// 开始选单打开
        /// </summary>
        public const string UI_START_MAIN_OPEN = "UIStartMainOpen";
        /// <summary>
        /// 开始选单按钮被点击
        /// </summary>
        public const string UI_START_MAIN_BUTTON_START = "UIStartMainButtonStart";


        /// <summary>
        /// 选择阶段相关
        /// </summary>
        public const string UI_CHOOSE_STAGE = "UIChooseStage";
        /// <summary>
        /// 选择阶段开始
        /// </summary>
        public const string UI_CHOOSE_STAGE_START = "UIChooseStageStart";
        /// <summary>
        /// 选择界面打开
        /// </summary>
        public const string UI_CHOOSE_STAGE_OPEN = "UIChooseStageOpen";
        /// <summary>
        /// 选择界面读取卡牌列表
        /// </summary>
        public const string UI_CHOOSE_STAGE_LOAD_CARD_INFO = "UIChooseStageLoadCardInfo";
        /// <summary>
        /// 选择界面读取卡牌列表
        /// </summary>
        public const string UI_CHOOSE_STAGE_LOAD_CARD_ENTRY = "UIChooseStageLoadCardEntry";

        /// <summary>
        /// 选择了某一张卡
        /// </summary>
        public const string UI_CHOOSE_STAGE_ONE_CARD = "UIChooseStageOneCard";
        /// <summary>
        /// 下一个用户选择
        /// </summary>
        public const string UI_CHOOSE_STAGE_NEXT_ONE_SHIP = "UIChooseStageNextOneShip";


        /// <summary>
        /// 卡组列表相关
        /// </summary>
        public const string UI_CARD_DECK_LIST = "UICardDeckList";
        /// <summary>
        /// 卡组列表打开
        /// </summary>
        public const string UI_CARD_DECK_LIST_OPEN = "UICardDeckListOpen";
        /// <summary>
        /// 卡组列表读取
        /// </summary>
        public const string UI_CARD_DECK_LIST_LOAD = "UICardDeckListLoad";




    }
}
