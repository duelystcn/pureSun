using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.GameModelInfo;
using Assets.Scripts.OrderSystem.Model.Hex;
using Assets.Scripts.OrderSystem.Model.Minion;
using Assets.Scripts.OrderSystem.Model.OperateSystem;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.Util;
using Assets.Scripts.OrderSystem.View.UIView;
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
            GameModelProxy gameModelProxy = 
                Facade.RetrieveProxy(GameModelProxy.NAME) as GameModelProxy;

            //获取当前操作玩家
            string playerCode = circuitProxy.GetNowPlayerCode();
            PlayerItem playerItem = playerGroupProxy.getPlayerByPlayerCode(playerCode);
            HandCellItem chooseHand = operateSystemProxy.operateSystemItem.onChooseHandCellItem;

            switch (notification.Type) {

                //判断手牌是否可用
                case OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE:
                    string playerCodeHandCanUseJudge = notification.Body as string;
                    PlayerItem playerItemHandCanUseJudge = playerGroupProxy.playerGroup.playerItems[playerCodeHandCanUseJudge];
                    playerItemHandCanUseJudge.ChangeHandCardCanUse();
                    SendNotification(HandSystemEvent.HAND_CHANGE, playerItem.handGridItem.handCells, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_CAN_USE_JUDGE, playerCode));
                    break;
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
                            EffectInfo effectInfo = effectInfoProxy.GetDepthCloneEffectByName(handCellItem.cardEntry.cardInfo.targetEffectInfo);
                            effectInfo.player = playerItem;
                            if (effectInfo.targetSetList[0].target == "Minion") {
                                //传入效果，根据效果目标进行筛选渲染
                                SendNotification(MinionSystemEvent.MINION_SYS, effectInfo, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT);
                            }
                            break;
                    }
                    break;
                //划线结束选择了战场
                case OperateSystemEvent.OPERATE_SYS_DRAW_END_HEX:
                    HexCellItem hexCellItem = notification.Body as HexCellItem;
                    HexCoordinates index = hexCellItem.coordinates;
                    UtilityLog.Log("玩家【" + playerCode + "】尝试操作手牌，手牌种类为【" + chooseHand.cardEntry.WhichCard + "】", LogUtType.Operate);
                    //判断状态
                    switch (operateSystemProxy.operateSystemItem.operateModeType) {
                        //手牌使用状态
                        case OperateSystemItem.OperateType.HandUse:
                            switch (chooseHand.cardEntry.WhichCard) {
                                case CardEntry.CardType.ResourceCard:
                                    UtilityLog.Log("玩家【" + playerCode + "】进行操作手牌，手牌种类为【" + chooseHand.cardEntry.WhichCard + "】", LogUtType.Operate);
                                    chooseHand.cardEntry.player = playerItem;
                                    //执行卡牌
                                    SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, chooseHand.cardEntry, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD);
                                    break;
                                case CardEntry.CardType.MinionCard:
                                    //检查是否可用释放
                                    bool canUse = playerItem.CheckOneCardCanUse(chooseHand.cardEntry);
                                    bool canCall = playerItem.CheckOneCellCanCall(hexCellItem.coordinates);
                                    //检查所选格子是否可用召唤
                                    if (canUse && canCall)
                                    {
                                        //通知战场层去除渲染
                                        SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL);
                                        SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE_EXE_OVER);
                                        minionGridProxy.AddOneMinionByHand(index, chooseHand);
                                    }
                                    else {
                                        SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_DRAW_END_NULL);
                                    }
                                    break;
                                case CardEntry.CardType.TacticsCard:
                                    //如果成功释放了，还需要去除掉目标渲染
                                    bool checkUseSuccess = false;
                                    string targetType = "Null";
                                    //如果存在目标，则需要先选择目标再释放
                                    if (chooseHand.cardEntry.cardInfo.targetEffectInfo != "Null")
                                    {
                                        bool hasTarget = false;
                                        EffectInfo targetEffectInfo = effectInfoProxy.GetDepthCloneEffectByName(chooseHand.cardEntry.cardInfo.targetEffectInfo);
                                        targetEffectInfo.player = playerItem;
                                        if (targetEffectInfo.targetSetList[0].target == "Minion")
                                        {
                                            targetType = "Minion";
                                            //判断格子上是否有生物
                                            MinionCellItem minionCellItem = minionGridProxy.GetMinionCellItemByIndex(index);
                                            if (minionCellItem != null)
                                            {
                                                //检查是否满足效果释放条件
                                                if (targetEffectInfo.checkEffectToTargetMinionCellItem(minionCellItem))
                                                {
                                                    //确认目标
                                                    chooseHand.cardEntry.targetMinionCellItem = minionCellItem;
                                                    hasTarget = true;
                                                }

                                            }
                                        }
                                        if (hasTarget)
                                        {
                                            checkUseSuccess = true;
                                            //执行卡牌
                                            SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, chooseHand.cardEntry, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD);
                                        }
                                      
                                    }
                                    //如果不需要选择目标或者需要选择多个目标则先执行
                                    else {
                                        //执行卡牌
                                        SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, chooseHand.cardEntry, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD);
                                    }
                                    if (checkUseSuccess) {
                                        if (targetType == "Minion") {
                                            //取消渲染
                                            SendNotification(MinionSystemEvent.MINION_SYS, null, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE);
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE_EXE_OVER:
                    //如果是战术，资源牌，放入墓地
                    if (chooseHand.cardEntry.WhichCard == CardEntry.CardType.ResourceCard || chooseHand.cardEntry.WhichCard == CardEntry.CardType.TacticsCard) {
                        //在墓地添加手牌
                        playerItem.AddOneCardToGraveyard(chooseHand.cardEntry);
                    }
                    //减少费用
                    playerItem.ChangeManaUsableByUseHand(chooseHand);
                    //移除手牌
                    playerItem.RemoveOneCardByUse(chooseHand);
                    //结束，改变模式为初始，清除手牌
                    operateSystemProxy.IntoModeClose();
                    break;
                case OperateSystemEvent.OPERATE_SYS_DRAW_END_NULL:
                    //什么都没选
                    switch (operateSystemProxy.operateSystemItem.operateModeType)
                    {
                        //手牌使用状态
                        case OperateSystemItem.OperateType.HandUse:
                           
                            //手牌回复原位
                            SendNotification(HandSystemEvent.HAND_CHANGE, chooseHand, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_UNCHECK_STATUS, playerItem.playerCode));
                            switch (chooseHand.cardEntry.WhichCard)
                            {
                                case CardEntry.CardType.MinionCard:
                                    //通知战场层取消渲染
                                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL);
                                    operateSystemProxy.IntoModeClose();
                                    break;
                                case CardEntry.CardType.TacticsCard:
                                    EffectInfo effectInfo = effectInfoProxy.GetDepthCloneEffectByName(chooseHand.cardEntry.cardInfo.effectCodeList[0]);
                                    if (effectInfo.targetSetList[0].target == "Minion")
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
                    if (effectCard.effectCodeList.Length > 1) {
                        UtilityLog.LogError("this chooseEffect has many Effect");
                    }
                    EffectInfo chooseEffect = effectInfoProxy.GetDepthCloneEffectByName(effectCard.effectCodeList[0]);
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = effectInfoProxy.effectSysItem.effectInfos[n];
                        if (effect.effectInfoStage == EffectInfoStage.ConfirmingTarget)
                        {
                            if (effect.targetSetList[0].target == "ChooseEffect")
                            {
                                //确认目标
                                effect.targetSetList[0].targetEffectInfos.Add(chooseEffect);
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
                                SendNotification(
                                    UIViewSystemEvent.UI_VIEW_CURRENT,
                                    UIViewConfig.getNameStrByUIViewName(UIViewName.ChooseStage),
                                    StringUtil.GetNTByNotificationTypeAndUIViewNameAndMaskLayer(
                                        UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW,
                                        UIViewConfig.getNameStrByUIViewName(UIViewName.ChooseStage),
                                        "N"
                                    )
                                );


                                //返回继续执行效果选择的信号
                                SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_OBJECT);
                            }
                            else {
                                UtilityLog.LogError("this effectType is not ChooseEffect");
                            }
                           
                        }
                    }
                    break;
                //打开了墓地
                case OperateSystemEvent.OPERATE_SYS_GRAVEYARD_LIST_LOAD:
                    string playerCodeOpenGraveyard = notification.Body as string;
                    PlayerItem playerItemOpenGraveyard = playerGroupProxy.getPlayerByPlayerCode(playerCodeOpenGraveyard);
                    //获取墓地的牌，并发送给前台
                    SendNotification(
                            UIViewSystemEvent.UI_VIEW_CURRENT,
                            playerItemOpenGraveyard.cardGraveyard,
                            StringUtil.GetNTByNotificationTypeAndUIViewNameAndMaskLayer(
                                UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                UIViewConfig.getNameStrByUIViewName(UIViewName.GraveyardListView),
                                "Y"
                                )
                            );

                    break;
            }
        }
        //
      
    }
}
