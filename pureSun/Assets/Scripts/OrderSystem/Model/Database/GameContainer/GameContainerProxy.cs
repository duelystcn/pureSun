using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using PureMVC.Patterns.Proxy;
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.GameContainer
{
    public class GameContainerProxy : Proxy
    {
        public new const string NAME = "GameContainerProxy";

        public List<GameContainerItem> gameContainerItemList;

        public GameContainerProxy() : base(NAME)
        {
            gameContainerItemList = new List<GameContainerItem>();
            base.Data = gameContainerItemList;
        }

        //为一个玩家创建所必须的容器
        public void CreateNecessaryContainer(PlayerItem playerItem, string[] gameContainerTypeList) {
            foreach (string gameContainerType in gameContainerTypeList) {
                gameContainerItemList.Add(CreateContainerByPlayerItemAndGameContainerType(playerItem, gameContainerType));
            }
        }
        //根据玩家和类型创造出容器
        public GameContainerItem CreateContainerByPlayerItemAndGameContainerType(PlayerItem playerItem, string gameContainerType) {
            GameContainerItem gameContainerItem = new GameContainerItem();
            gameContainerItem.gameContainerType = gameContainerType;
            gameContainerItem.controllerPlayerItem = playerItem;
            gameContainerItem.Create();
            return gameContainerItem;

        }
        //向指定容器中添加一张固定的牌
        public void AddCardByPlayerItemAndGameContainerType(PlayerItem playerItem, string gameContainerType,CardEntry cardEntry) {
            GameContainerItem gameContainerItem = GetGameContainerItemByPlayerItemAndGameContainerType(playerItem, gameContainerType);
            cardEntry.gameContainerType = gameContainerType;
            gameContainerItem.PutOneCard(cardEntry);
        }

        //向指定容器中添加多张固定的牌
        public void AddCardListByPlayerItemAndGameContainerType(PlayerItem playerItem, string gameContainerType, List<CardEntry> cardEntryList)
        {
            GameContainerItem gameContainerItem = GetGameContainerItemByPlayerItemAndGameContainerType(playerItem, gameContainerType);
            foreach (CardEntry cardEntry in cardEntryList) {
                cardEntry.gameContainerType = gameContainerType;
                gameContainerItem.PutOneCard(cardEntry);
            }

        }
        //根据玩家和容器类别获取容器
        public GameContainerItem GetGameContainerItemByPlayerItemAndGameContainerType(PlayerItem playerItem, string gameContainerType)
        {
            GameContainerItem returnGameContainerItem = null;
            foreach (GameContainerItem gameContainerItem in gameContainerItemList) {
                if (gameContainerItem.controllerPlayerItem == playerItem && gameContainerItem.gameContainerType == gameContainerType) {
                    returnGameContainerItem = gameContainerItem;
                }
            }
            return returnGameContainerItem;
        }
        //将一张卡从一个容器移动到另外一个容器
        public bool MoveOneCardFromOldeContainerItemToNeweContainerItem(CardEntry cardEntry,string newGameContainerType) {
            //获取所有者
            PlayerItem playerItem = cardEntry.controllerPlayerItem;
            //获取旧的容器类别
            string oldGameContainerType = cardEntry.gameContainerType;
            //获取旧的容器
            GameContainerItem oldGameContainerItem = GetGameContainerItemByPlayerItemAndGameContainerType(playerItem, oldGameContainerType);
            //旧的容器移除这张卡
            oldGameContainerItem.RemoveOneCardEntry(cardEntry);
            //新的容器添加这张卡
            AddCardByPlayerItemAndGameContainerType(playerItem, newGameContainerType, cardEntry);

            return true;
        }

    }
}
