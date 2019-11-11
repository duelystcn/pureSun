

namespace Assets.Scripts.OrderSystem.Event
{
    class OperateSystemEvent
    {
        //操作逻辑
        public const string OPERATE_SYS = "OperateSys";
        //类型
        //操作逻辑开始
        public const string OPERATE_SYS_HAND_CHOOSE = "OperateSysHandChoose";

        //效果执行完毕，清除手牌
        public const string OPERATE_SYS_HAND_CHOOSE_EXE_OVER = "OperateSysHandChooseExeOver";




        //划线结束，选择了区域格子
        public const string OPERATE_SYS_DRAW_END_HEX = "OperateSysDrawEndHex";
        //划线结束，选择了献祭区域
        public const string OPERATE_SYS_DRAW_END_CIRCUIT = "OperateSysDrawEndCircuit";
        //划线结束，什么都没选
        public const string OPERATE_SYS_DRAW_END_NULL = "OperateSysDrawEndNull";

       





        //操作逻辑-划线
        public const string OPERATE_TRAIL_DRAW = "OperateTrailDraw";
        //类型
        //划线开始
        public const string OPERATE_TRAIL_DRAW_START = "OperateTrailDrawStart";
        //添加对战时划线组件
        public const string OPERATE_TRAIL_DRAW_CREATE = "OperateTrailDrawCreate";

        //选择了一个效果
        public const string OPERATE_SYS_CHOOSE_ONE_EFFECT = "OperateSysChooseOneEffect";

        //选择了一个效果的选项
        public const string OPERATE_SYS_CHOOSE_ONE_USER_SELECTION_ITEM = "OperateSysChooseOneUserSelectionItem ";

        /// <summary>
        /// 监听器相关，需要重新渲染手牌可用，暂时放在这里
        /// </summary>
        public const string OPERATE_SYS_HAND_CAN_USE_JUDGE = "OperateSysHandCanUseJudge";


        /// <summary>
        /// 墓地列表打开
        /// </summary>
        public const string OPERATE_SYS_GRAVEYARD_LIST_LOAD = "OperateSysGraveyardListLoad";

    }
}
