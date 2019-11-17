using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.Effect;
using Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS;
using Assets.Scripts.OrderSystem.Model.Database.GameContainer;
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
            QuestStageCircuitProxy questStageCircuitProxy =
                Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            PlayerGroupProxy playerGroupProxy =
                Facade.RetrieveProxy(PlayerGroupProxy.NAME) as PlayerGroupProxy;
            CardDbProxy cardDbProxy =
                Facade.RetrieveProxy(CardDbProxy.NAME) as CardDbProxy;
            EffectInfoProxy effectInfoProxy =
                Facade.RetrieveProxy(EffectInfoProxy.NAME) as EffectInfoProxy;
            GameModelProxy gameModelProxy = 
                Facade.RetrieveProxy(GameModelProxy.NAME) as GameModelProxy;
            GameContainerProxy gameContainerProxy =
                Facade.RetrieveProxy(GameContainerProxy.NAME) as GameContainerProxy;
            HexGridProxy hexGridProxy =
                  Facade.RetrieveProxy(HexGridProxy.NAME) as HexGridProxy;

            //获取当前操作玩家
            string playerCode = questStageCircuitProxy.GetNowHaveStagePlayerCode();
            if (playerCode == null) {
                return;
            }
            PlayerItem playerItem = playerGroupProxy.getPlayerByPlayerCode(playerCode);
            CardEntry chooseHand = operateSystemProxy.operateSystemItem.onChooseCardEntry;

            switch (notification.Type) {

                //判断手牌是否可用
                case OperateSystemEvent.OPERATE_SYS_HAND_CAN_USE_JUDGE:
                    string playerCodeHandCanUseJudge = notification.Body as string;
                    PlayerItem playerItemHandCanUseJudge = playerGroupProxy.playerGroup.playerItems[playerCodeHandCanUseJudge];
                    GameContainerItem gameContainerItem = gameContainerProxy.GetGameContainerItemByPlayerItemAndGameContainerType(playerItemHandCanUseJudge, "CardHand");
                    gameContainerItem.ChangeHandCardCanUse(questStageCircuitProxy.circuitItem);
                    SendNotification(HandSystemEvent.HAND_CHANGE, gameContainerItem.cardEntryList, StringUtil.GetNTByNotificationTypeAndPlayerCode(HandSystemEvent.HAND_CHANGE_CAN_USE_JUDGE, playerCode));
                    break;
                //选中战场上一个生物
                case OperateSystemEvent.OPERATE_SYS_POINTER_DOWN_ONE_MINION:
                    CardEntry downMinionCard = notification.Body as CardEntry;
                    operateSystemProxy.IntoModeByType(downMinionCard, playerItem, OperateSystemItem.OperateType.MinionControling);
                    //渲染可移动区域
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, operateSystemProxy.operateSystemItem, HexSystemEvent.HEX_VIEW_RENDER_CAN_MOVE_AND_ATK);
                    //消息通知-划线组件激活
                    SendNotification(OperateSystemEvent.OPERATE_TRAIL_DRAW, null, OperateSystemEvent.OPERATE_TRAIL_DRAW_START);

                    break;
                //选中手牌
                case OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE:
                    CardEntry handCellItem = notification.Body as CardEntry;
                    operateSystemProxy.IntoModeByType(handCellItem, playerItem, OperateSystemItem.OperateType.HandUse);
                    switch (handCellItem.WhichCard)
                    {
                        case CardEntry.CardType.MinionCard:
                            //渲染可召唤区域
                            SendNotification(HexSystemEvent.HEX_VIEW_SYS, operateSystemProxy.operateSystemItem, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL);
                            break;
                        case CardEntry.CardType.TacticsCard:
                            //渲染可释放
                            //获取效果信息
                            //判断是否存在用于渲染的目标效果
                            if (handCellItem.cardInfo.targetSetToChooseList != null) {
                                foreach (string targetSetToChooseCode in handCellItem.cardInfo.targetSetToChooseList) {
                                    TargetSet targetSetToChoose = effectInfoProxy.GetDepthCloneTargetSetByName(targetSetToChooseCode);
                                    foreach (TargetClaim targetClaim in targetSetToChoose.targetClaims)
                                    {
                                        if (targetClaim.claim == "Owner")
                                        {
                                            if (targetClaim.content == "Myself")
                                            {
                                                targetClaim.result.Add(playerItem.playerCode);
                                            }

                                        }
                                    }
                                    if (targetSetToChoose.target == "Minion")
                                    {
                                        //传入效果，根据效果目标进行筛选渲染
                                        SendNotification(MinionSystemEvent.MINION_SYS, targetSetToChoose, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT);
                                    }

                                }
                            }

                            break;
                    }
                    break;
                //划线结束选择了战场
                case OperateSystemEvent.OPERATE_SYS_DRAW_END_HEX:
                    HexCellItem hexCellItem = notification.Body as HexCellItem;
                    HexCoordinates index = hexCellItem.coordinates;
                    //通知战场层去除渲染
                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL);
                    //判断状态
                    if (operateSystemProxy.operateSystemItem.operateModeType == OperateSystemItem.OperateType.HandUse) {
                        UtilityLog.Log("玩家【" + playerCode + "】尝试操作手牌，手牌种类为【" + chooseHand.WhichCard + "】", LogUtType.Operate);
                        //如果成功释放了，还需要去除掉目标渲染
                        bool checkUseSuccess = false;
                        string targetType = "Null";
                        switch (chooseHand.WhichCard)
                        {
                            case CardEntry.CardType.ResourceCard:
                                UtilityLog.Log("玩家【" + playerCode + "】进行操作手牌，手牌种类为【" + chooseHand.WhichCard + "】", LogUtType.Operate);
                                checkUseSuccess = true;
                                //执行卡牌
                                //SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, chooseHand, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD);
                                break;
                            case CardEntry.CardType.MinionCard:
                                //检查是否可用释放
                                bool canUse = playerItem.CheckOneCardCanUse(chooseHand, questStageCircuitProxy.circuitItem);
                                bool canCall = playerItem.CheckOneCellCanCall(hexCellItem.coordinates);
                                //检查所选格子是否可用召唤
                                if (canUse && canCall)
                                {
                                    checkUseSuccess = true;
                                    chooseHand.nowIndex = index;
                                }
                                else
                                {
                                    SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_DRAW_END_NULL);
                                }
                                break;
                            case CardEntry.CardType.TacticsCard:
                                //如果存在目标，则需要先选择目标再释放
                                if (chooseHand.cardInfo.targetSetToChooseList != null)
                                {
                                    bool hasTarget = false;
                                    foreach (string targetSetToChooseCode in chooseHand.cardInfo.targetSetToChooseList)
                                    {
                                        TargetSet targetSetToChoose = effectInfoProxy.GetDepthCloneTargetSetByName(targetSetToChooseCode);
                                        if (targetSetToChoose.target == "Minion")
                                        {
                                            targetType = "Minion";
                                            //判断格子上是否有生物
                                            CardEntry minionCellItem = gameContainerProxy.CheckHasCardEntryByGameContainerTypeAndHexCoordinates("CardBattlefield", index);
                                            if (minionCellItem != null)
                                            {
                                                //检查是否满足效果释放条件
                                                if (targetSetToChoose.checkEffectToTargetMinionCellItem(minionCellItem))
                                                {
                                                    //确认目标
                                                    chooseHand.targetBasicGameDto = minionCellItem;
                                                    hasTarget = true;
                                                }

                                            }
                                        }
                                    }
                                    if (hasTarget)
                                    {
                                        checkUseSuccess = true;
                                    }
                                }
                                //如果不需要选择目标或者需要选择多个目标则先执行
                                else
                                {
                                    checkUseSuccess = true;
                                }
                                break;
                        }
                        if (checkUseSuccess)
                        {
                            //减少费用-预先减少
                            playerItem.ChangeManaUsableByUseHand(chooseHand);
                            chooseHand.nextGameContainerType = "CardIsReleasing";
                            chooseHand.ttNeedChangeGameContainerType(chooseHand);
                            //减少费用-预先减少
                            playerItem.ChangeManaUsableByUseHand(chooseHand);
                            if (targetType == "Minion")
                            {
                                //取消渲染
                                SendNotification(MinionSystemEvent.MINION_SYS, null, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE);
                            }
                            //执行卡牌
                            SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, chooseHand, EffectExecutionEvent.EFFECT_EXECUTION_SYS_EXE_CARD);
                        }
                    } 
                    else if (operateSystemProxy.operateSystemItem.operateModeType == OperateSystemItem.OperateType.MinionControling) {
                        UtilityLog.Log("玩家【" + playerCode + "】尝试操作生物", LogUtType.Special);
                        if (operateSystemProxy.operateSystemItem.onChooseCardEntry.canBeMovedCellMap.ContainsKey(index)) {
                            HexCellItem endHexCellItem = operateSystemProxy.operateSystemItem.onChooseCardEntry.canBeMovedCellMap[index];
                            List<HexCellItem> cellRoute = new List<HexCellItem>();
                            cellRoute.Add(endHexCellItem);
                            bool isOver = false;
                            while (!isOver) {
                                if (endHexCellItem.pathfindingLastCell.coordinates.X == operateSystemProxy.operateSystemItem.onChooseCardEntry.nowIndex.X
                                    && endHexCellItem.pathfindingLastCell.coordinates.Z == operateSystemProxy.operateSystemItem.onChooseCardEntry.nowIndex.Z) {
                                    isOver = true;
                                }
                                else {
                                    cellRoute.Add(endHexCellItem.pathfindingLastCell);
                                    endHexCellItem = endHexCellItem.pathfindingLastCell;
                                }
                            }
                            //路径是倒的，需要换成正序的
                            List<HexCellItem> positiveCellRoute = new List<HexCellItem>();
                            for (int n = cellRoute.Count - 1; n > -1; n--) {
                                positiveCellRoute.Add(cellRoute[n]);
                            }
                            operateSystemProxy.operateSystemItem.onChooseCardEntry.cellRoute = positiveCellRoute;
                            effectInfoProxy.effectSysItem.showEffectNum++;
                            operateSystemProxy.operateSystemItem.onChooseCardEntry.MoveToTargetHexCoordinates(index);
                        }
                    }
                   
                    break;
                case OperateSystemEvent.OPERATE_SYS_HAND_CHOOSE_EXE_OVER:
                    //如果是战术，资源牌，放入墓地
                    if (chooseHand.WhichCard == CardEntry.CardType.ResourceCard || chooseHand.WhichCard == CardEntry.CardType.TacticsCard) {
                        //在墓地添加手牌
                        chooseHand.nextGameContainerType = "CardGraveyard";
                        chooseHand.ttNeedChangeGameContainerType(chooseHand);
                    } else if (chooseHand.WhichCard == CardEntry.CardType.MinionCard) {
                        gameContainerProxy.AddOneMinionByCard(chooseHand.nowIndex, chooseHand);
                        chooseHand.nextGameContainerType = "CardBattlefield";
                        chooseHand.ttNeedChangeGameContainerType(chooseHand);
                    }
                    //移除手牌
                    playerItem.RemoveOneCardByUse(chooseHand);
                    UtilityLog.Log("手牌【" + chooseHand.name + "】使用完毕：", LogUtType.Operate);
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
                            switch (chooseHand.WhichCard)
                            {
                                case CardEntry.CardType.MinionCard:
                                    //通知战场层取消渲染
                                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_RENDER_CAN_CALL_CANCEL);
                                    operateSystemProxy.IntoModeClose();
                                    break;
                                case CardEntry.CardType.TacticsCard:
                                    EffectInfo effectInfo = effectInfoProxy.GetDepthCloneEffectByName(chooseHand.cardInfo.effectCodeList[0]);
                                    foreach (TargetSet targetSet in effectInfo.operationalTarget.selectTargetList) {
                                        if (targetSet.target == "Minion")
                                        {
                                            //取消渲染
                                            SendNotification(MinionSystemEvent.MINION_SYS, effectInfo, MinionSystemEvent.MINION_SYS_EFFECT_HIGHLIGHT_CLOSE);
                                        }
                                    }
                                   

                                    break;
                            }
                            break;

                    }
                    break;
                //选择一个生物作为某次选择的结果
                case OperateSystemEvent.OPERATE_SYS_CHOOSE_ONE_MINION:
                    CardEntry oneChooseMinionCellItem = notification.Body as CardEntry;
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = effectInfoProxy.effectSysItem.effectInfos[n];
                        if (effect.effectInfoStage == EffectInfoStage.ConfirmingTarget)
                        {
                            //遍历每一个前置效果
                            foreach (EffectInfo preEffect in effect.preEffectEntryList)
                            {
                                if (preEffect.effectInfoStage == EffectInfoStage.ConfirmingTarget)
                                {
                                    preEffect.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                                    preEffect.needPlayerToChooseTargetSet.targetMinionCellItems.Add(oneChooseMinionCellItem);
                                    break;
                                }
                            }
                            //遍历每一个后置效果
                            foreach (EffectInfo postEffect in effect.postEffectEntryList)
                            {
                                if (postEffect.effectInfoStage == EffectInfoStage.ConfirmingTarget)
                                {
                                    postEffect.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                                    postEffect.needPlayerToChooseTargetSet.targetMinionCellItems.Add(oneChooseMinionCellItem);
                                    break;
                                }
                            }
                            effectInfoProxy.effectSysItem.showEffectNum--;
                            //返回继续执行效果选择的信号
                            SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_TARGET);
                        }
                    }
                    break;
                //选择一个效果的某一个选项
                case OperateSystemEvent.OPERATE_SYS_CHOOSE_ONE_USER_SELECTION_ITEM:
                    OneUserSelectionItem oneUserSelectionItem = notification.Body as OneUserSelectionItem;
                    for (int n = 0; n < effectInfoProxy.effectSysItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = effectInfoProxy.effectSysItem.effectInfos[n];
                        if (effect.effectInfoStage == EffectInfoStage.AskTheUser) {
                            //遍历每一个前置效果
                            foreach (EffectInfo preEffect in effect.preEffectEntryList) {
                                if (preEffect.effectInfoStage  == EffectInfoStage.AskTheUser) {
                                    preEffect.userChooseExecution = oneUserSelectionItem.isExecute;
                                    preEffect.effectInfoStage = EffectInfoStage.AskTheUserOver;
                                    UtilityLog.Log("收到前台的反馈并修改了",LogUtType.Special);
                                    break;
                                }
                            }
                            effect.effectInfoStage = EffectInfoStage.AskTheUserOver;
                            effectInfoProxy.effectSysItem.showEffectNum--;
                            //返回继续执行效果选择的信号
                            SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_ASK_THE_USER);
                        }
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
                            foreach (TargetSet targetSet in effect.operationalTarget.selectTargetList)
                            {
                                if (targetSet.target == "ChooseEffect")
                                {
                                    //确认目标
                                    targetSet.targetEffectInfos.Add(chooseEffect);
                                    //设置为确认完毕
                                    effect.effectInfoStage = EffectInfoStage.ConfirmedTarget;
                                    //添加新效果
                                    //设置状态
                                    chooseEffect.effectInfoStage = EffectInfoStage.UnStart;
                                    //设置所有者,手牌操作模式，所有者是当前玩家
                                    chooseEffect.player = playerItem;
                                    //设置所属卡牌
                                    chooseEffect.cardEntry = chooseHand;
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
                                    SendNotification(EffectExecutionEvent.EFFECT_EXECUTION_SYS, null, EffectExecutionEvent.EFFECT_EXECUTION_SYS_FIND_TARGET);
                                    return;
                                }
                                else
                                {
                                    UtilityLog.LogError("this effectType is not ChooseEffect");
                                }
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
                            gameContainerProxy.GetGameContainerItemByPlayerItemAndGameContainerType(playerItemOpenGraveyard, "CardGraveyard"),
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
