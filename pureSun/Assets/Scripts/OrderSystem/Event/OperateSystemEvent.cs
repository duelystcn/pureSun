

namespace Assets.Scripts.OrderSystem.Event
{
    class OperateSystemEvent
    {
        //操作逻辑
        public const string OPERATE_SYS = "OperateSys";
        //类型
        //操作逻辑开始
        public const string OPERATE_SYS_HAND_CHOOSE = "OperateSysHandChoose";



        

        //划线结束，选择了区域格子
        public const string OPERATE_SYS_DRAW_END_HEX = "OperateSysDrawEndHex";
        //划线结束，选择了献祭区域
        public const string OPERATE_SYS_DRAW_END_CIRCUIT = "OperateSysDrawEndCircuit";
        //划线结束，什么都没选
        public const string OPERATE_SYS_DRAW_END_NULL = "OperateSysDrawEndNull";
        //开始寻找目标
        public const string OPERATE_SYS_FIND_TARGET = "OperateSysFindTarget";
        //开始执行效果
        public const string OPERATE_SYS_EXEC_EFFECT= "OperateSysExecEffect";




        //操作逻辑-划线
        public const string OPERATE_TRAIL_DRAW = "OperateTrailDraw";
        //类型
        //划线开始
        public const string OPERATE_TRAIL_DRAW_START = "OperateTrailDrawStart";
        //添加对战时划线组件
        public const string OPERATE_TRAIL_DRAW_CREATE = "OperateTrailDrawCreate";


    }
}
