using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
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
        //根据类别获取获取多个容器
        public List<GameContainerItem> GetGameContainerItemGameContainerType( string gameContainerType)
        {
            List<GameContainerItem> returnGameContainerItemList = new List<GameContainerItem>();
            foreach (GameContainerItem gameContainerItem in gameContainerItemList)
            {
                if ( gameContainerItem.gameContainerType == gameContainerType)
                {
                    returnGameContainerItemList.Add(gameContainerItem);
                }
            }
            return returnGameContainerItemList;
        }
        //根据类别获取获取多个容器的卡
        public List<CardEntry> GetCardEntryListByGameContainerType(string gameContainerType)
        {
            List<CardEntry> returnCardEntryList = new List<CardEntry>();
            foreach (GameContainerItem gameContainerItem in gameContainerItemList)
            {
                if (gameContainerItem.gameContainerType == gameContainerType)
                {
                    foreach (CardEntry cardEntry in gameContainerItem.cardEntryList) {
                        returnCardEntryList.Add(cardEntry);
                    }
                }
            }
            return returnCardEntryList;
        }
        //传入一个坐标检查这个坐标上是否有其他的卡
        public CardEntry CheckHasCardEntryByGameContainerTypeAndHexCoordinates(string gameContainerType,HexCoordinates hexCoordinates) {
            List<CardEntry> returnCardEntryList = this.GetCardEntryListByGameContainerType(gameContainerType);
            foreach (CardEntry cardEntry in returnCardEntryList)
            {
                if (cardEntry.nowIndex.X == hexCoordinates.X && cardEntry.nowIndex.Z == hexCoordinates.Z)
                {
                    return cardEntry;
                }
            }
            return null;
        }


        //将一张卡从一个容器移动到另外一个容器
        public bool MoveOneCardFromOldeContainerItemToNeweContainerItem(CardEntry cardEntry, string newGameContainerType) {
            UtilityLog.Log("【" + cardEntry.cardInfo.code + "】移动所在位置，从【" + cardEntry.gameContainerType + "】到【" + newGameContainerType+ "】",LogUtType.Effect);
            //获取所有者
            PlayerItem playerItem = cardEntry.controllerPlayerItem;
            //获取旧的容器类别
            string oldGameContainerType = cardEntry.gameContainerType;
            //获取旧的容器
            GameContainerItem oldGameContainerItem = GetGameContainerItemByPlayerItemAndGameContainerType(playerItem, oldGameContainerType);
            //旧的容器移除这张卡
            oldGameContainerItem.RemoveOneCardEntry(cardEntry);
            cardEntry.lastGameContainerType = cardEntry.gameContainerType;
            //新的容器添加这张卡
            AddCardByPlayerItemAndGameContainerType(playerItem, newGameContainerType, cardEntry);
            cardEntry.ttCardChangeGameContainerType(cardEntry);
            return true;
        }
        //根据指定牌添加生物
        public void AddOneMinionByCard(HexCoordinates index, CardEntry cardEntry)
        {
            //判断这个坐标上是否已经有生物存在
            if (CheckHasCardEntryByGameContainerTypeAndHexCoordinates("CardBattlefield",index) != null)
            {
                UtilityLog.LogError("该位置已存在生物");
                return;
            }

            cardEntry.dtoType = "Minion";

            cardEntry.IsEffectTarget = false;

            cardEntry.nowIndex = index;
            cardEntry.cardEntryVariableAttributeMap.CreateVariableAttributeByOriginalValueAndCodeAndBetterAndAutoRestore("Atk", cardEntry.atk, true);
            cardEntry.cardEntryVariableAttributeMap.CreateVariableAttributeByOriginalValueAndCodeAndBetterAndAutoRestore("Def", cardEntry.def, true);
            AddTimeTrigger(cardEntry);
            SendNotification(MinionSystemEvent.MINION_VIEW, cardEntry, MinionSystemEvent.MINION_VIEW_ADD_ONE_MINION);
        }
        //添加信号发射
        public void AddTimeTrigger(CardEntry minionCellItem)
        {
            minionCellItem.ttAttributeChange = () =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_MINION_CHANGE_ATTRIBUTE);
            };

            //buff发生了变化
            minionCellItem.ttBuffChange = () =>
            {
                SendNotification(UIViewSystemEvent.UI_ONE_CARD_ALL_INFO, minionCellItem, UIViewSystemEvent.UI_ONE_CARD_ALL_INFO_BUFF_CHANGE);
            };
            //buff需要被移除
            minionCellItem.ttBuffNeedRemove = () =>
            {
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_MINION_BUFF_NEED_REMOVE);
            };
            //生物准备发起一次攻击
            minionCellItem.ttLaunchAnAttack = () => {
                UtilityLog.Log("玩家【" + minionCellItem.controllerPlayerItem.playerCode + "】的生物【" + minionCellItem.name + "】发起一次攻击", LogUtType.Attack);
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_LAUNCH_AN_ATTACK);
            };
            //生物进行一次攻击
            minionCellItem.ttExecuteAnAttack = () =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_ATTACK_TARGET_MINION);
            };
            //生物发起一次移动
            minionCellItem.ttLaunchAnMove = () =>
            {
                UtilityLog.Log("玩家【" + minionCellItem.controllerPlayerItem.playerCode + "】的生物【" + minionCellItem.name + "】发起一次移动", LogUtType.Attack);
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_LAUNCH_AN_MOVE);
            };
            //生物进行一次移动
            minionCellItem.ttExecuteAnMove = () =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_MOVE_TARGET_HEX_CELL);
            };
            //生物死亡
            minionCellItem.ttCardMinionIsDead = () =>
            {
                SendNotification(MinionSystemEvent.MINION_VIEW, minionCellItem, MinionSystemEvent.MINION_VIEW_ONE_MINION_IS_DEAD);
                SendNotification(MinionSystemEvent.MINION_SYS, minionCellItem, MinionSystemEvent.MINION_SYS_ONE_MINION_IS_DEAD);
            };
            //生物进入战场
            minionCellItem.ttMinionIntoBattlefield = () => {
                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, minionCellItem, EffectExecutionEvent.EFFECT_EXECUTION_SYS_MINION_ENTER_THE_BATTLEFIELD);

            };
            //生物被牺牲
            minionCellItem.ttMinionToSacrifice = () =>
            {
                minionCellItem.ttCardMinionIsDead();
            };
        }

    }
}
