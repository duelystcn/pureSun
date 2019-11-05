﻿/*************************************************************************************
     * 类 名 称：       PlayerSite
     * 文 件 名：       PlayerSite
     * 创建时间：       2019-10-28
     * 作    者：       chenxi
     * 说   明：        实体类，储存从GameModel.json中读取到的值
     * 修改时间：
     * 修 改 人：
    *************************************************************************************/

namespace Assets.Scripts.OrderSystem.Model.Database.GameModelInfo
{
    public class GM_PlayerSite
    {
        //攻击方向,使用坐标表示
        public GM_CellCoordinate attackDefaultDirection { get; set; }
        //移动方向
        public GM_CellCoordinate moveDefaultDirection { get; set; }
        //默认可召唤区域
        public string[] canCallRegionCodes { get; set; }
        //区域信息
        public GM_CellRegion[] cellRegionList { get; set; }
        //可移动区域
        public string[]  canMoveRegionCodes { get; set; }
    }
}
