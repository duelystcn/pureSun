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

        //把卡组区域的第一张牌移动到手牌区域
        public const string GAME_CONTAINER_SYS_DECK_TOP_CARD_MOVE_HAND = "GameContainerSysDeckTopCardMoveHand";

        //一张卡需要添加到监听器
        public const string GAME_CONTAINER_SYS_CARD_NEED_ADD_TO_TTS = "GameContainerSysCardNeedAddToTTS";
    }
}
