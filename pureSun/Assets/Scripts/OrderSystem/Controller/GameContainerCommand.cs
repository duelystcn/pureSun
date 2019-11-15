using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class GameContainerCommand : SimpleCommand
    {
       
        public override void Execute(INotification notification) {
            GameContainerProxy gameContainerProxy =
                Facade.RetrieveProxy(GameContainerProxy.NAME) as GameContainerProxy;
            EffectInfoProxy effectInfoProxy =
                Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            QuestStageCircuitProxy questStageCircuitProxy =
                Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            switch (notification.Type) {
                case GameContainerEvent.GAME_CONTAINER_SYS_CARD_NEED_MOVE:
                    CardEntry cardEntryNeedMoveToHand = notification.Body as CardEntry;
                    gameContainerProxy.MoveOneCardFromOldeContainerItemToNeweContainerItem(cardEntryNeedMoveToHand, cardEntryNeedMoveToHand.nextGameContainerType);
                    break;
                case GameContainerEvent.GAME_CONTAINER_SYS_CARD_NEED_ADD_TO_TTS:
                    CardEntry cardEntryNeedAddToTTS = notification.Body as CardEntry;
                    //获取卡牌的效果，如果是持续效果则添加到全局监听中
                    foreach (string effectCode in cardEntryNeedAddToTTS.effectCodeList)
                    {
                        EffectInfo oneEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(effectCode);
                        if (oneEffectInfo.impactType == "Continue")
                        {
                            oneEffectInfo.player = cardEntryNeedAddToTTS.controllerPlayerItem;
                            oneEffectInfo.cardEntry = cardEntryNeedAddToTTS;
                            questStageCircuitProxy.circuitItem.putOneEffectInfoInActiveMap(oneEffectInfo, effectInfoProxy.effectSysItem.impactTimeTriggerMap);
                        }

                    }
                    break;



            }


        }
    }
}
