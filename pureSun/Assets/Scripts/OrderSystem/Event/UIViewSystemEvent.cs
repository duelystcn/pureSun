

namespace Assets.Scripts.OrderSystem.Event
{
    class UIViewSystemEvent
    {
        /// <summary>
        /// UI相关
        /// </summary>
        public const string UI_VIEW = "UIView";


        /// <summary>
        /// 综合通用指令，打开关闭窗口这些通用的
        /// </summary>
        public const string UI_VIEW_CURRENT = "UIViewCurrent";

        /// <summary>
        /// 综合通用指令，打开窗口
        /// </summary>
        public const string UI_VIEW_CURRENT_OPEN_ONE_VIEW = "UIViewCurrentOpenOneView";

        /// <summary>
        /// 综合通用指令，关闭窗口
        /// </summary>
        public const string UI_VIEW_CURRENT_CLOSE_ONE_VIEW = "UIViewCurrentCloseOneView";




        /// <summary>
        /// 尝试控制转发？
        /// </summary>
        public const string UI_VIEW_ZF_HAND_CHANGE = "UIView=>handChange";



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
        /// 选择界面读取卡牌列表
        /// </summary>
        public const string UI_CHOOSE_STAGE_LOAD_CARD_ENTRY = "UIChooseStageLoadCardEntry";
        /// <summary>
        /// 选择了某一张卡
        /// </summary>
        public const string UI_CHOOSE_STAGE_ONE_CARD = "UIChooseStageOneCard";
        /// <summary>
        /// 船只选择完毕播放船只动画
        /// </summary>
        public const string UI_CHOOSE_STAGE_ONE_SHIP_CARD_ANIMATION = "UIChooseStageOneShipCardAnimation";

        /// <summary>
        /// 船只选择完毕播放船只动画完毕
        /// </summary>
        public const string UI_CHOOSE_STAGE_ONE_SHIP_CARD_ANIMATION_OVER = "UIChooseStageOneShipCardAnimationOver";


        /// <summary>
        /// 组合选择阶段相关
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE = "UIChooseMakeStage";
        /// <summary>
        /// 组合选择阶段初始化
        /// </summary>
        public const string UI_CHOOSE_MAKE_STAGE_INIT = "UIChooseMakeStageInit";
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
        /// 小局阶段相关
        /// </summary>
        public const string UI_QUEST_STAGE = "UIQuestStage";
        /// <summary>
        /// 小局阶段标准开始
        /// </summary>
        public const string UI_QUEST_STAGE_START = "UIQuestStageStart";
        /// <summary>
        /// 小局阶段特殊开始（测试用例，谜题等，跳过分发手牌，读取已存在的信息）
        /// </summary>
        public const string UI_QUEST_STAGE_START_SPECIAL = "UIQuestStageStartSpecial";

        /// <summary>
        /// 回合阶段相关
        /// </summary>
        public const string UI_QUEST_TURN_STAGE = "UIQuestTurnStage";
        /// <summary>
        /// 开始一个回合
        /// </summary>
        public const string UI_QUEST_TURN_STAGE_START_OF_TRUN = "UIQuestStageStartOfTrun";
        /// <summary>
        /// 开始一个回合-指定阶段
        /// </summary>
        public const string UI_QUEST_TURN_STAGE_ASSIGN_START_OF_TRUN = "UIQuestStageAssignStartOfTrun";
        /// <summary>
        /// 结束一个回合
        /// </summary>
        public const string UI_QUEST_TURN_STAGE_END_OF_TRUN = "UIQuestStageEndOfTrun";
        /// <summary>
        ///开始一个阶段
        /// </summary>
        public const string UI_QUEST_TURN_STAGE_START_OF_STAGE= "UIQuestStageStartOfStage";
        /// <summary>
        ///执行一个阶段
        /// </summary>
        public const string UI_QUEST_TURN_STAGE_EXECUTION_OF_STAGE = "UIQuestStageExecutionOfStage";
        /// <summary>
        ///结束一个阶段
        /// </summary>
        public const string UI_QUEST_TURN_STAGE_END_OF_STAGE = "UIQuestStageEndOfStage";
        /// <summary>
        ///通知当前效果已结算完毕，可以判断是否结束当前阶段的状态
        /// </summary>
        public const string UI_QUEST_TURN_STAGE_NEED_CHECK_END_STAGE_STATE = "UIQuestStageNeedCheckEndStageState";



        /// <summary>
        /// 卡牌详细信息窗口
        /// </summary>
        public const string UI_ONE_CARD_ALL_INFO = "UIOneCardAllInfo";
        /// <summary>
        /// 卡牌详细信息窗口打开
        /// </summary>
        public const string UI_ONE_CARD_ALL_INFO_BUFF_CHANGE = "UIOneCardAllInfoBuffChange";
     


        /// <summary>
        /// 进行用户选择操作
        /// </summary>
        public const string UI_USER_OPERAT = "UIUserOperat";
        /// <summary>
        /// 进行用户选择效果界面
        /// </summary>
        public const string UI_USER_OPERAT_CHOOSE_EFFECT = "UIUserOperatChoooseEffect";

      


        /// <summary>
        /// 动画播放相关
        /// </summary>
        public const string UI_ANIMATION_SYS= "UIAnimationtSys";
        /// <summary>
        /// UI动画播放
        /// </summary>
        public const string UI_ANIMATION_SYS_START = "UIAnimationtSysStart";
        /// <summary>
        /// 转发完成UI动画继续播放
        /// </summary>
        public const string UI_ANIMATION_SYS_ZF_OVER_START = "UIAnimationtSysZFOverStart";


        /// <summary>
        /// 费用显示相关
        /// </summary>
        public const string UI_MANA_INFA_SYS = "UIManaInfoSys";
        /// <summary>
        /// 费用显示初始化
        /// </summary>
        public const string UI_MANA_INFA_SYS_INIT = "UIManaInfoSysInit";

        /// <summary>
        /// 费用上限发生变化
        /// </summary>
        public const string UI_MANA_INFA_SYS_NUM_CHANGE = "UIManaInfoSysNumChange";
      

        /// <summary>
        /// 科技显示相关
        /// </summary>
        public const string UI_TRAIT_COMBINATION_SYS = "UITraitCombinationSys";
        /// <summary>
        /// 科技显示打开
        /// </summary>
        public const string UI_TRAIT_COMBINATION_SYS_OPEN = "UITraitCombinationSysOpen";
        /// <summary>
        /// 科技显示初始化
        /// </summary>
        public const string UI_TRAIT_COMBINATION_SYS_INIT = "UITraitCombinationSysInit";
        /// <summary>
        /// 科技增加
        /// </summary>
        public const string UI_TRAIT_COMBINATION_SYS_ADD = "UITraitCombinationSysAdd";

        /// <summary>
        /// 玩家显示相关
        /// </summary>
        public const string UI_PLAYER_SHOW_SYS = "UIPlayerShowSys";
        /// <summary>
        /// 玩家显示船只
        /// </summary>
        public const string UI_PLAYER_SHOW_SHIP_CARD = "UIPlayerShowShipCard";


        /// <summary>
        /// 效果展示列表相关
        /// </summary>
        public const string UI_EFFECT_DISPLAY_SYS = "UIEffectDisplaySys";
        /// <summary>
        /// 效果展示列表打开
        /// </summary>
        public const string UI_EFFECT_DISPLAY_SYS_OPEN = "UIEffectDisplaySysOpen";
        /// <summary>
        /// 效果展示列表关闭
        /// </summary>
        public const string UI_EFFECT_DISPLAY_SYS_CLOSE = "UIEffectDisplaySysClose";
        /// <summary>
        /// 效果展示列表放入一个效果
        /// </summary>
        public const string UI_EFFECT_DISPLAY_SYS_PUT_ONE_EFFECT = "UIEffectDisplaySysPutOneEffect";
        /// <summary>
        /// 一个效果需要用户选择是否执行
        /// </summary>
        public const string UI_EFFECT_DISPLAY_SYS_ONE_EFFECT_NEED_CHOOSE_EXE= "UIEffectDisplaySysOneEffectNeedChooseExe";
        /// <summary>
        /// 一个效果需要用户选择目标
        /// </summary>
        public const string UI_EFFECT_DISPLAY_SYS_ONE_EFFECT_NEED_CHOOSE_TARGET = "UIEffectDisplaySysOneEffectNeedChooseTarget";
        /// <summary>
        /// 隐藏效果展示列表
        /// </summary>
        public const string UI_EFFECT_DISPLAY_SYS_TO_HIDE = "UIEffectDisplaySysToHide";


        /// <summary>
        /// 玩家分数显示相关
        /// </summary>
        public const string UI_PLAYER_SCORE_SHOW_SYS = "UIPlayerScoreShowSys";
        /// <summary>
        /// 玩家分数变化
        /// </summary>
        public const string UI_PLAYER_SCORE_SHOW_SYS_CHANGE = "UIPlayerScoreShowSysChange";


        /// <summary>
        /// 回合结束按钮显示相关
        /// </summary>
        public const string UI_NEXT_TURN_SHOW_SYS = "UINextTurnShowSys";
        /// <summary>
        /// 回合结束按钮显示
        /// </summary>
        public const string UI_NEXT_TURN_SHOW_SYS_OPEN = "UINextTurnShowSysOpen";
        /// <summary>
        /// 回合结束按钮显示
        /// </summary>
        public const string UI_NEXT_TURN_SHOW_SYS_SHOW = "UINextTurnShowSysShow";

        /// <summary>
        /// 测试案例相关
        /// </summary>
        public const string UI_TEST_CASE_SYS = "UITestCaseSys";
        /// <summary>
        /// 测试案例选择窗口打开
        /// </summary>
        public const string UI_TEST_CASE_VIEW_OPEN = "UITestCaseViewOpen";

        /// <summary>
        /// 墓地详细显示相关
        /// </summary>
        public const string UI_GRAVEYARD_LIST_VIEW = "UIGraveyardListView";
        /// <summary>
        /// 测试案例选择窗口打开
        /// </summary>
        public const string UI_GRAVEYARD_LIST_VIEW_OPEN = "UIGraveyardListViewOpen";


        /// <summary>
        /// 卡牌发生变化
        /// </summary>
        public const string UI_CARD_ENTRY_SYS = "UICardEntrySys";
        /// <summary>
        /// 卡牌归属地发生变化
        /// </summary>
        public const string UI_CARD_ENTRY_SYS_CHANGE_GAME_CONTAINER_TYPE = "UICardEntrySysChangeGameContainerType";
        /// <summary>
        /// 卡牌正在被使用中
        /// </summary>
        public const string UI_CARD_ENTRY_SYS_CARD_NEED_HIDE_IN_VIEW = "UICardEntrySysCardNeedHideInView";


        /// <summary>
        /// 回合阶段相关页面
        /// </summary>
        public const string UI_TURN_STAGE_SYS = "UITurnStageSys";

        /// <summary>
        /// 回合阶段发生变化
        /// </summary>
        public const string UI_TURN_STAGE_SYS_STAGE_CHANGE = "UITurnStageSys";




    }
}
