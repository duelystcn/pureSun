

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
        /// 开始选单关闭
        /// </summary>
        public const string UI_START_MAIN_CLOSE = "UIStartMainClose";
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
        /// 选择界面关闭
        /// </summary>
        public const string UI_CHOOSE_STAGE_CLOSE = "UIChooseStageClose";

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
        /// 组合选择阶段相关
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE = "UIChooseMakeStage";
        /// <summary>
        /// 组合选择阶段相关
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE_OPEN = "UIChooseMakeStageOpen";
        /// <summary>
        /// 组合选择阶段选择了某一张卡
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE_ONE_CARD = "UIChooseMakeStageOneCard";
        /// <summary>
        /// 组合选择阶段购买成功
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE_ONE_CARD_SUCCESS = "UIChooseMakeStageOneCardSuccess";
        /// <summary>
        /// 组合选择阶段购买失败
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE_ONE_CARD_FAILURE = "UIChooseMakeStageOneCardFailure";
        /// <summary>
        /// 读取下一组数据
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE_LOAD_NEXT_LIST = "UIChooseMakeStageLoadNextList";
        /// <summary>
        /// 读取关闭窗口
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE_CLOSE = "UIChooseMakeStageClose";



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


        /// <summary>
        /// 小回合阶段相关
        /// </summary>
        public const string UI_QUEST_STAGE = "UIQuestStage";
        /// <summary>
        /// 小回合阶段相关
        /// </summary>
        public const string UI_QUEST_STAGE_START = "UIQuestStageStart";

        /// <summary>
        /// 卡牌详细信息窗口
        /// </summary>
        public const string UI_ONE_CARD_ALL_INFO = "UIOneCardAllInfo";
        /// <summary>
        /// 卡牌详细信息窗口打开
        /// </summary>
        public const string UI_ONE_CARD_ALL_INFO_OPEN = "UIOneCardAllInfoOpen";
        /// <summary>
        /// 卡牌详细信息窗口关闭
        /// </summary>
        public const string UI_ONE_CARD_ALL_INFO_CLOSE = "UIOneCardAllInfoClose";


        /// <summary>
        /// 进行用户选择操作
        /// </summary>
        public const string UI_USER_OPERAT = "UIUserOperat";
        /// <summary>
        /// 进行用户选择效果界面
        /// </summary>
        public const string UI_USER_OPERAT_CHOOSE_EFFECT = "UIUserOperatChoooseEffect";





    }
}
