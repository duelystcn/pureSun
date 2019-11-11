
using Assets.Scripts.OrderSystem.Model.Player;

namespace Assets.Scripts.OrderSystem.Model.Common.BasicGame
{
    public class BasicGameDto
    {
        //唯一标识符
        public string code;
        //实体类型
        public string dtoType = "BasicGameDto";
        //uuid
        public string uuid;
        //当前所有权
        public PlayerItem controllerPlayerItem;

    }
}
