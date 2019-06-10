﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        //手牌操作
        public const string HAND_OPERATE = "handOperate";
        //手牌选中
        public const string HAND_OPERATE_ONCHOOSE = "handOperateOnChoose";
        //手牌使用完毕
        public const string HAND_CHANGE_USE_OVER = "handChangeUseOver";
    }
}