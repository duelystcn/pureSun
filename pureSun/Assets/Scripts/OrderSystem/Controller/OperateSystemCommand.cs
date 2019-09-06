using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class OperateSystemCommand : SimpleCommand
    {

        public override void Execute(INotification notification)
        {
            OperateSystemProxy operateSystemProxy =
                Facade.RetrieveProxy(OperateSystemProxy.NAME) as OperateSystemProxy;
            QuestStageCircuitProxy circuitProxy =
                Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            PlayerGroupProxy playerGroupProxy =
                Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            CardDbProxy cardDbProxy =
                Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            MinionGridProxy minionGridProxy = 
                Facade.RetrieveProxy(MinionGridProxy.NAME) as MinionGridProxy;
            EffectInfoProxy effectInfoProxy =
                Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;


            //获取当前操作玩家
            string playerCode = circuitProxy.GetNowPlayerCode();
            PlayerItem playerItem = playerGroupProxy.getPlayerByPlayerCode(playerCode);
            HandCellItem chooseHand = operateSystemProxy.operateSystemItem.onChooseHandCellItem;

            switch (notification.Type) {
                //选中手牌
                case OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE:
                    HandCellItem handCellItem = notification.Body as HandCellItem;
                    operateSystemProxy.IntoModeHandUse(handCellItem, playerItem);
                    switch (handCellItem.cardEntry.WhichCard)
                    {
                        case CardEntry.CardType.MinionCard:
                            //渲染可召唤区域
                            SendNotification(HexSystemEvent.HEX_VIEW_SYS, operateSystemProxy.operateSystemItem, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL);
                            break;
                        case CardEntry.CardType.TacticsCard:
                            //渲染可释放
                            //获取效果信息
                            EffectInfo effectInfo = effectInfoProxy.GetDepthCloneEffectByName(handCellItem.cardEntry.cardInfo.effectName[0]);
                            if (effectInfo.target == "ONE_MINION") {
                                //传入效果，根据效果目标进行筛选渲染
                                SendNotification(MinionSystemEvent.MINION_SYS, effectInfo, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT);
                            }
                            break;
                    }
                    break;
                //划线结束选择了战场
                case OperateSystemEvent.OPERATE_SYS_DRAW_END_HEX:
                    HexCellItem hexCellItem = notification.Body as HexCellItem;
                    int index = HexUtil.GetIndexFromModeAndHex(operateSystemProxy.hexModelInfo, hexCellItem.coordinates);
                    //判断状态
                    switch (operateSystemProxy.operateSystemItem.operateModeType) {
                        //手牌使用状态
                        case OperateSystemItem.OperateType.HandUse:
                            switch (chooseHand.cardEntry.WhichCard) {
                                case CardEntry.CardType.ResourceCard:
                                    chooseHand.cardEntry.player = playerItem;
                                    //执行卡牌
                                    SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, chooseHand.cardEntry, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD);
                                   
                                    break;
                                case CardEntry.CardType.MinionCard:
                                    //检查是否可用释放
                                    bool canUse = playerItem.checkOneCardCanUse(chooseHand.cardEntry);
                                    bool canCall = playerItem.checkOneCellCanCall(hexCellItem.coordinates);
                                    //检查所选格子是否可用召唤
                                    if (canUse && canCall)
                                    {
                                        minionGridProxy.minionGridItem.AddOneMinion(index, chooseHand.cardEntry);
                                        //移除手牌
                                        playerItem.RemoveOneCard(chooseHand);
                                        //通知生物层发生变更重新渲染
                                        SendNotification(MinionSystemEvent.MINION_VIEW, minionGridProxy.minionGridItem, MinionSystemEvent.MINION_VIEW_CHANGE_OVER);
                                        //通知战场层去除渲染
                                        SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL);
                                        //结束，改变模式为初始，清除手牌
                                        operateSystemProxy.IntoModeClose();
                                    }
                                    else {
                                        SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_DRAW_END_NULL);
                                    }
                                    break;
                                case CardEntry.CardType.TacticsCard:
                                    //获取效果信息
                                    EffectInfo effectInfo = effectInfoProxy.GetDepthCloneEffectByName(chooseHand.cardEntry.cardInfo.effectName[0]);
                                    if (effectInfo.target == "ONE_MINION")
                                    {
                                        UtilityLog.Log("index:"+index);
                                        //判断格子上是否有生物
                                        MinionCellItem minionCellItem = minionGridProxy.GetMinionCellItemByIndex(index);
                                        if (minionCellItem!=null) {
                                            //检查是否满足效果释放条件
                                            //释放
                                            effectInfo.TargetMinionOne(minionCellItem);

                                        }
                                    }
                                    //结束，改变模式为初始，清除手牌
                                    operateSystemProxy.IntoModeClose();
                                    break;
                            }
                            break;
                    }
                    break;
                case OperateSystemEvent.OPERATE_SYS_DRAW_END_NULL:
                    //什么都没选
                    switch (operateSystemProxy.operateSystemItem.operateModeType)
                    {
                        //手牌使用状态
                        case OperateSystemItem.OperateType.HandUse:
                            //手牌回复原位
                            SendNotification(HandSystemEvent.HAND_CHANGE, chooseHand, HandSystemEvent.HAND_CHANGE_UNCHECK_STATUS);
                            switch (chooseHand.cardEntry.WhichCard)
                            {
                                case CardEntry.CardType.MinionCard:
                                    //通知战场层取消渲染
                                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL);
                                    operateSystemProxy.IntoModeClose();
                                    break;
                                case CardEntry.CardType.TacticsCard:
                                    EffectInfo effectInfo = effectInfoProxy.GetDepthCloneEffectByName(chooseHand.cardEntry.cardInfo.effectName[0]);
                                    if (effectInfo.target == "ONE_MINION")
                                    {
                                        //取消渲染
                                        SendNotification(MinionSystemEvent.MINION_SYS, effectInfo, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE);
                                    }

                                    break;
                            }
                            break;

                    }
                    break;
                //选择一个效果类型，选择了目标
                case OperateSystemEvent.OPERATE_SYS_CHOOSE_ONE_EFFECT:
                    CardEntry effectCard = notification.Body as CardEntry;
                    //逻辑上可以确定只能有一个效果字段？
                    if (effectCard.effectName.Length > 1) {
                        UtilityLog.LogError("this chooseEffect has many Effect");
                    }
                    EffectInfo chooseEffect = effectInfoProxy.GetDepthCloneEffectByName(effectCard.effectName[0]);
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = effectInfoProxy.effectSysItem.effectInfos[n];
                        if (effect.effectInfoStage == EffectInfoStage.Confirming)
                        {
                            if (effect.target == "ChooseEffect")
                            {
                                //确认目标
                                effect.TargetEffectInfos.Add(chooseEffect);
                                //设置为确认完毕
                                effect.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                                //添加新效果
                                //设置状态
                                chooseEffect.effectInfoStage = EffectInfoStage.UnStart;
                                //设置所有者,手牌操作模式，所有者是当前玩家
                                chooseEffect.player = playerItem;
                                //设置所属卡牌
                                chooseEffect.cardEntry = chooseHand.cardEntry;
                                //将这个效果添加到队列中
                                effectInfoProxy.effectSysItem.effectInfos.Add(chooseEffect);
                                //返回一个选择完毕的信号
                                SendNotification(UIViewSystemEvent.UI_USER_OPERAT, null,
                                         StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_USER_OPERAT_CHOOSE_EFFECT_OVER, playerItem.playerCode));
                                //返回继续执行效果选择的信号
                                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_TARGET);
                            }
                            else {
                                UtilityLog.LogError("this effectType is not ChooseEffect");
                            }
                           
                        }
                    }
                    break;
            }
        }
        //
      
    }
}
