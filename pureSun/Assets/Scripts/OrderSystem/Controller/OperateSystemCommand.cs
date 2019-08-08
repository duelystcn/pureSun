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
                            SendNotification(HexSystemEvent.HEX_VIEW_SYS, operateSystemProxy.operateSystemItem, HexSystemEvent.HEX_VIEW_SYS_CHANGE);
                            break;
                        case CardEntry.CardType.TacticsCard:
                            //渲染可释放
                            //获取效果信息
                            EffectInfo effectInfo = effectInfoProxy.effectSysItem.effectInfoMap[handCellItem.cardEntry.cardInfo.effectName[0]];
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
                                    List<EffectInfo> effectInfos = new List<EffectInfo>();
                                    //遍历效果，进行预释放
                                    foreach (string effectName in chooseHand.cardEntry.effectName) {
                                        EffectInfo oneEffectInfo = effectInfoProxy.effectSysItem.effectInfoMap[effectName];
                                        //设置状态
                                        oneEffectInfo.effectInfoStage = EffectInfoStage.UnStart;
                                        //设置所有者,手牌操作模式，所有者是当前玩家
                                        oneEffectInfo.player = playerItem;
                                        //设置所属卡牌
                                        oneEffectInfo.cardEntry = chooseHand.cardEntry;
                                        effectInfos.Add(oneEffectInfo);
                                    }
                                    //存入效果，进行结算
                                    operateSystemProxy.IntoModeCardSettle(chooseHand.cardEntry, effectInfos);
                                    SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_FIND_TARGET);
                                    break;
                                case CardEntry.CardType.MinionCard:
                                    //获取当前玩家虚拟坐标
                                    HexCoordinates hexCoordinates = operateSystemProxy.operateSystemItem.playerItem.hexCoordinates;
                                    int row = 999;
                                    if (hexCoordinates.Z < 0)
                                    {
                                        row = hexCoordinates.Z + 1;
                                    }
                                    if (hexCoordinates.Z > 0)
                                    {
                                        row = hexCoordinates.Z - 1;
                                    }
                                    if (hexCellItem.coordinates.Z + Mathf.RoundToInt(hexCellItem.coordinates.X / 2) == row)
                                    {
                                        minionGridProxy.minionGridItem.AddOneMinion(index, chooseHand.cardEntry);
                                        //通知手牌层发生变更重新渲染
                                        //SendNotification(HandSystemEvent.HAND_CHANGE, chooseHand, HandSystemEvent.HAND_CHANGE_USE_OVER);
                                        //通知生物层发生变更重新渲染
                                        SendNotification(MinionSystemEvent.MINION_VIEW, minionGridProxy.minionGridItem, MinionSystemEvent.MINION_VIEW_CHANGE_OVER);
                                        //通知战场层去除渲染
                                        SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_SYS_CLOSE_HIGHLIGHT);
                                        //结束，改变模式为初始，清除手牌
                                        operateSystemProxy.IntoModeClose();
                                    }
                                    else {
                                        SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_DRAW_END_NULL);
                                    }
                                    break;
                                case CardEntry.CardType.TacticsCard:
                                    //获取效果信息
                                    EffectInfo effectInfo = effectInfoProxy.effectSysItem.effectInfoMap[chooseHand.cardEntry.cardInfo.effectName[0]];
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
                            switch (chooseHand.cardEntry.WhichCard)
                            {
                                case CardEntry.CardType.MinionCard:
                                    //通知战场层取消渲染
                                    SendNotification(HexSystemEvent.HEX_VIEW_SYS, null, HexSystemEvent.HEX_VIEW_SYS_CLOSE_HIGHLIGHT);
                                    operateSystemProxy.IntoModeClose();
                                    break;
                                case CardEntry.CardType.TacticsCard:
                                    EffectInfo effectInfo = effectInfoProxy.effectSysItem.effectInfoMap[chooseHand.cardEntry.cardInfo.effectName[0]];
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
                //选择目标阶段
                case OperateSystemEvent.OPERATE_SYS_FIND_TARGET:
                    for (int n = 0; n < operateSystemProxy.operateSystemItem.effectInfos.Count; n++) {
                        UtilityLog.Log("正在选择目标的效果" + operateSystemProxy.operateSystemItem.effectInfos[n].name);
                        EffectInfo effect = operateSystemProxy.operateSystemItem.effectInfos[n];
                        if (effect.effectInfoStage == EffectInfoStage.UnStart)
                        {
                            ExecutionEffectFindTarget(effect, playerGroupProxy);
                            //插入了用户操作
                            if (effect.effectInfoStage == EffectInfoStage.Confirming) {
                                break;
                            }
                        }
                        //如果是正在选择，逻辑上不存在这种情况
                        else if (effect.effectInfoStage == EffectInfoStage.Confirming) {
                            UtilityLog.LogError("Should not exist Confirming EffectInfoStage");
                        }
                    }
                    //判断是否所有效果都存在了目标
                    bool allEffectHasTarget = true;
                    for (int n = 0; n < operateSystemProxy.operateSystemItem.effectInfos.Count; n++)
                    {
                        if (operateSystemProxy.operateSystemItem.effectInfos[n].effectInfoStage != EffectInfoStage.ConfirmedTarget) {
                            allEffectHasTarget = false;
                        }
                    }
                    //开始执行效果
                    if (allEffectHasTarget) {
                        SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_EXEC_EFFECT);
                    }
                    break;
                case OperateSystemEvent.OPERATE_SYS_EXEC_EFFECT:
                    for (int n = 0; n < operateSystemProxy.operateSystemItem.effectInfos.Count; n++)
                    {
                        ExecutionEffectContent(operateSystemProxy.operateSystemItem.effectInfos[n]);
                       
                    }
                    //判断是否所有效果都执行完毕了
                    bool allEffectHasFinished = true;
                    for (int n = 0; n < operateSystemProxy.operateSystemItem.effectInfos.Count; n++)
                    {
                        if (operateSystemProxy.operateSystemItem.effectInfos[n].effectInfoStage != EffectInfoStage.Finished)
                        {
                            allEffectHasFinished = false;
                        }
                    }
                    if (allEffectHasFinished)
                    {
                        //判断状态
                        switch (operateSystemProxy.operateSystemItem.operateModeType)
                        {
                            //手牌使用状态
                            case OperateSystemItem.OperateType.HandUse:
                                //返回手牌使用完毕的信息，移除手牌
                                playerGroupProxy.getPlayerByPlayerCode(operateSystemProxy.operateSystemItem.playerItem.playerCode).RemoveOneCard(operateSystemProxy.operateSystemItem.onChooseHandCellItem);
                                //结束，改变模式为初始，清除手牌
                                operateSystemProxy.IntoModeClose();
                                break;
                        }
                    }
                    else {
                        UtilityLog.LogError("效果执行失败");
                    }
                    break;
                //选择一个效果类型，选择了目标
                case OperateSystemEvent.OPERATE_SYS_CHOOSE_ONE_EFFECT:
                    CardEntry effectCard = notification.Body as CardEntry;
                    //逻辑上可以确定只能有一个效果字段？
                    if (effectCard.effectName.Length > 1) {
                        UtilityLog.LogError("this chooseEffect has many Effect");
                    }
                    EffectInfo chooseEffect = effectInfoProxy.effectSysItem.effectInfoMap[effectCard.effectName[0]];
                    for (int n = 0; n < operateSystemProxy.operateSystemItem.effectInfos.Count; n++)
                    {
                        EffectInfo effect = operateSystemProxy.operateSystemItem.effectInfos[n];
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
                                operateSystemProxy.operateSystemItem.effectInfos.Add(chooseEffect);
                                //返回一个选择完毕的信号
                                SendNotification(UIViewSystemEvent.UI_USER_OPERAT, null,
                                         StringUtil.NotificationTypeAddPlayerCode(UIViewSystemEvent.UI_USER_OPERAT_CHOOSE_EFFECT_OVER, playerItem.playerCode));
                                //返回继续执行效果选择的信号
                                SendNotification(OperateSystemEvent.OPERATE_SYS, null, OperateSystemEvent.OPERATE_SYS_FIND_TARGET);
                            }
                            else {
                                UtilityLog.LogError("this effectType is not ChooseEffect");
                            }
                           
                        }
                    }
                    break;
            }
        }
        //执行效果
        public void ExecutionEffectContent(EffectInfo effectInfo) {
            effectInfo.effectInfoStage = EffectInfoStage.Executing;
            switch (effectInfo.target)
            {
                //效果选择
                case "ChooseEffect":
                    //无需执行，玩家已操作

                    break;
                //玩家
                case "Player":
                    effectInfo.TargetPlayerList(effectInfo.TargetPlayerItems);
                    break;

            }
            effectInfo.effectInfoStage = EffectInfoStage.Finished;

        }
        //为效果选择目标
        public void ExecutionEffectFindTarget(EffectInfo effectInfo, PlayerGroupProxy playerGroupProxy)
        {
            effectInfo.effectInfoStage = EffectInfoStage.Confirming;
            //条件
            string[] targetClaims = effectInfo.targetClaims;
            //条件内容
            string[] targetClaimsContents = effectInfo.targetClaimsContents;
            //目标玩家
            PlayerItem targetPlayer = null;
            //类型
            switch (effectInfo.target)
            {
                //效果选择
                case "ChooseEffect":
                    //获取玩家，根据条件筛选出复合条件的释放者和选择者
                    //筛选结果
                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values) {
                        for (int n = 0; n < targetClaims.Length; n++)
                        {
                            //判断所有权
                            if (targetClaims[n] == "Owner") {
                                //是自己选
                                if (targetClaimsContents[n] == "Myself") {
                                    if (playerItem.playerCode == effectInfo.player.playerCode)
                                    {
                                        targetPlayer = playerItem;
                                    }
                                }
                            }
                        }
                    }
                    if (targetPlayer != null)
                    {
                        effectInfo.chooseByPlayer = targetPlayer;
                        //发布用户需要选择信号
                        SendNotification(LogicalSysEvent.LOGICAL_SYS, effectInfo, LogicalSysEvent.LOGICAL_SYS_CHOOSE_EFFECT);
                        //
                    }
                    else
                    {
                        UtilityLog.Log("no player can ChooseEffect");
                    }

                    break;
                //玩家
                case "Player":
                    //获取玩家，根据条件筛选出复合条件的释放者和选择者
                    //筛选结果
                    foreach (PlayerItem playerItem in playerGroupProxy.playerGroup.playerItems.Values)
                    {
                        for (int n = 0; n < targetClaims.Length; n++)
                        {
                            //判断所有权
                            if (targetClaims[n] == "Owner")
                            {
                                //是自己选
                                if (targetClaimsContents[n] == "Myself")
                                {
                                    if (playerItem.playerCode == effectInfo.player.playerCode)
                                    {
                                        targetPlayer = playerItem;
                                    }
                                }
                            }
                        }
                    }
                    if (targetPlayer != null)
                    {
                        //玩家确认
                        effectInfo.TargetPlayerItems.Add(targetPlayer);
                        effectInfo.effectInfoStage = EffectInfoStage.ConfirmedTarget;

                    }
                    else
                    {
                        UtilityLog.Log("no player can add TargetPlayerItems");
                    }
                    break;

            }
        }
    }
}
