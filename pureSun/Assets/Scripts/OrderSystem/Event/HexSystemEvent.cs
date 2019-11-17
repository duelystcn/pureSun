namespace Assets.Scripts.OrderSystem.Event
{
    class HexSystemEvent
    {
        //战场视图相关
        public const string HEX_VIEW_SYS = "hexViewSys";

        //战场视图开始渲染
        public const string HEX_VIEW_SYS_SHOW_START = "hexViewSysShowStart";

        public const string HEX_VIEW_SYS_SHOW = "hexViewSysShow";


        //渲染可召唤区域
        public const string HEX_VIEW_RENDER_CAN_CALL = "hexViewRenderCanCall";
        //渲染可召唤区域
        public const string HEX_VIEW_RENDER_CAN_MOVE_AND_ATK = "hexViewRenderCanMoveAndAtk";
        //渲染可召唤区域完毕
        public const string HEX_VIEW_RENDER_CAN_CALL_OVER = "hexViewRenderCanCallOver";
        //取消渲染可召唤区域
        public const string HEX_VIEW_RENDER_CAN_CALL_CANCEL = "hexViewRenderCanCallCancel";
        //取消渲染可召唤区域完毕
        public const string HEX_VIEW_RENDER_CAN_CALL_CANCEL_OVER = "hexViewRenderCanCallCancelOver";

        //战场视图发生了变化
        public const string HEX_VIEW_SYS_CHANGE = "hexViewSysChange";
        //取消战场高亮提示
        public const string HEX_VIEW_SYS_CLOSE_HIGHLIGHT = "hexViewSysCloseHighlight";


        //一个card可能进入战场可能离开战场
        public const string HEX_VIEW_SYS_CARD_CHANGE_GAME_CONTAINER_TYPE = "hexViewSysCardChangeGameConTainerType";
    }
}
