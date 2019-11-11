using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.OrderSystem.Event
{
    public class GameContainerEvent
    {

        //卡牌-区域之间的操作
        public const string GAME_CONTAINER_SYS = "GameContainerSys";

        //卡牌-区域之间的操作，获取某个区域的第一张牌，到另外一个区域
        public const string GAME_CONTAINER_SYS_CARD_NEED_MOVE = "GameContainerSysCardNeedMove";
    }
}
