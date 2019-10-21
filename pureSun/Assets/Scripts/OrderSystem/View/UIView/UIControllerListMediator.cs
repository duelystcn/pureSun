using Assets.Scripts.OrderSystem.Common;
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Database.TestCase;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.Model.Player.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.Animation;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.ChooseMakeStage;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.PlayerComponent;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.TestCase;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView.TraitCombination;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using OrderSystem;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.OrderSystem.View.UIView
{
    public class UIControllerListMediator : MediatorExpand
    {
        public new const string NAME = "UIControllerListMediator";



        private UIControllerListView UIControllerLIst
        {
            get { return (UIControllerListView)base.ViewComponent; }
        }

        public UIControllerListMediator(UIControllerListView UIControllerLIstView) : base(NAME, UIControllerLIstView)
        {
        }
       

        //注册时执行
        public override void OnRegister()
        {
            base.OnRegister();
            UIControllerLIst.AchieveUIControllerListView();

        }

        //监听
        public override string[] ListNotificationInterests()
        {
            List<string> notificationList = new List<string>();

            notificationList.Add(UIViewSystemEvent.UI_VIEW_CURRENT);

            notificationList.Add(UIViewSystemEvent.UI_CHOOSE_STAGE);
            notificationList.Add(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE);
            notificationList.Add(UIViewSystemEvent.UI_CARD_DECK_LIST);
            notificationList.Add(UIViewSystemEvent.UI_ONE_CARD_ALL_INFO);
            notificationList.Add(UIViewSystemEvent.UI_USER_OPERAT);
            notificationList.Add(UIViewSystemEvent.UI_ANIMATION_SYS);
            notificationList.Add(UIViewSystemEvent.UI_MANA_INFA_SYS);
            notificationList.Add(UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS);
            notificationList.Add(UIViewSystemEvent.UI_PLAYER_SHOW_SYS);
            notificationList.Add(UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS);
            notificationList.Add(UIViewSystemEvent.UI_PLAYER_SCORE_SHOW_SYS);
            notificationList.Add(UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS);
            notificationList.Add(UIViewSystemEvent.UI_TEST_CASE_SYS);

            //转发监听
            notificationList.Add(UIViewSystemEvent.UI_VIEW_ZF_HAND_CHANGE);

            AddCommonNotificationInterests(notificationList);
            return notificationList.ToArray();
        }



        public override void HandleNotification(INotification notification)
        {
            if (notification.Name == UIViewSystemEvent.UI_ANIMATION_SYS && notification.Type == UIViewSystemEvent.UI_ANIMATION_SYS_START)
            {
                DoExceHandleNotification();
            }
            else if (notification.Name == UIViewSystemEvent.UI_ANIMATION_SYS && notification.Type == UIViewSystemEvent.UI_ANIMATION_SYS_ZF_OVER_START)
            {
                exceINotification = false;
                DoExceHandleNotification();
            }
            else if (notification.Name == OrderSystemEvent.CLINET_SYS)
            {
                //客户端监听发放，不做处理
                HandleNotificationCommon(notification);
            }
            else
            {
                notificationQueue.Enqueue(notification);
                DoExceHandleNotification();
            }

        }

        public override void ExceHandleNotification(INotification notification)
        {
            //判断是否是转发内容
            if (notification.Name.Contains("=>"))
            {
                SendNotification(StringUtil.GeNotificationNameForNN(notification.Name), notification.Body, notification.Type);
                return;
            }

            // 处理公共请求
            HandleNotificationCommon(notification);


            ViewChooseStage viewChooseStage = null;
            List<CardDeckList> cardDeckListLoad = new List<CardDeckList>();
            CardMoveAnimation cardMoveAnimation = null;
            //回调函数
            UnityAction callBack = () =>
            {
                exceINotification = false;
                SendNotification(UIViewSystemEvent.UI_ANIMATION_SYS, null, UIViewSystemEvent.UI_ANIMATION_SYS_START);
            };
            bool callBackDelay = false;

            switch (notification.Name)
            {
                case UIViewSystemEvent.UI_ANIMATION_SYS:
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_ANIMATION_SYS_START:
                            DoExceHandleNotification();
                            break;
                    }
                    break;
                case UIViewSystemEvent.UI_VIEW_CURRENT:
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW:
                            UIViewName viewNameOpen = UIViewConfig.getUIViewNameByNameStr(parameterMap["UIViewName"]);
                            UIControllerLIst.ShowView(viewNameOpen);
                            //初始化页面
                            UIControllerLIst.GetViewByName<ViewBaseView>(viewNameOpen).InitViewForParameter(this, notification.Body);
                            //一些界面打开时需要初始化
                            switch (viewNameOpen) {
                                case UIViewName.CardDeckList:
                                    PlayerItem playerItem = notification.Body as PlayerItem;
                                    List<CardDeckList> cardDeckLists = UIControllerLIst.GetViewListByName<CardDeckList>(UIViewName.CardDeckList);
                                    for (int num = 0; num < cardDeckLists.Count; num++)
                                    {
                                        if (cardDeckLists[num].playerItem == null)
                                        {
                                            cardDeckLists[num].playerItem = playerItem;
                                            //设置位置
                                            //0在左，1在右
                                            //己方始终显示在左边
                                            Vector3 position = new Vector3();
                                            if (playerItem.playerCode == playerCode)
                                            {
                                                position.x = -600;

                                            }
                                            else
                                            {
                                                position.x = 430;

                                            }
                                            cardDeckLists[num].transform.localPosition = position;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW:
                            UIViewName viewNameClose = UIViewConfig.getUIViewNameByNameStr(notification.Body as string);
                            UIControllerLIst.HideView(viewNameClose);
                            break;
                    }
                    break;
                case UIViewSystemEvent.UI_CHOOSE_STAGE:
                    switch (notification.Type)
                    { 
                        case UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY:
                            viewChooseStage = UIControllerLIst.GetViewByName<ViewChooseStage>(UIViewName.ChooseStage);
                            List<CardEntry> shipCardEntryList = (List<CardEntry>)notification.Body;
                            //载入卡牌列表
                            viewChooseStage.LoadCardEntryList(shipCardEntryList);
                            //载入完成后绑定事件
                            foreach (CardIntactView cardIntactView in viewChooseStage.cardIntactViews)
                            {
                                //如果是自己的命令则绑定上点击事件
                                if (playerCode == playerCodeNotification)
                                {
                                    cardIntactView.OnClick = () =>
                                    {
                                        SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, cardIntactView.card, UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD);
                                    };
                                }
                                else
                                {
                                    //设置为空
                                    cardIntactView.OnClick = () =>
                                    {

                                    };
                                }

                            }
                            break;
                        case UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_SHIP_CARD_ANIMATION:
                            viewChooseStage = UIControllerLIst.GetViewByName<ViewChooseStage>(UIViewName.ChooseStage);
                            CardEntry choosedShipCard = notification.Body as CardEntry;
                            CardIntactView targetCardIntactView = null;
                            //找到这张组件
                            foreach (CardIntactView cardIntactView in viewChooseStage.cardIntactViews)
                            {
                                if (cardIntactView.card.uuid == choosedShipCard.uuid)
                                {
                                    cardDeckListLoad = UIControllerLIst.GetViewListByName<CardDeckList>(UIViewName.CardDeckList);
                                    cardMoveAnimation = UIControllerLIst.GetViewByName<CardMoveAnimation>(UIViewName.CardMoveAnimation);
                                    //执行动画，获得卡牌的位置，list的位置
                                    for (int num = 0; num < cardDeckListLoad.Count; num++)
                                    {
                                        //移动目标
                                        if (cardDeckListLoad[num].playerItem.playerCode == playerCodeNotification)
                                        {
                                            callBackDelay = true;
                                            cardMoveAnimation.MoveShipCardAnimation(cardIntactView, cardDeckListLoad[num], callBack);
                                        }
                                    }
                                }
                            }


                            break;
                    }
                    break;
                case UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE:
                    ViewChooseMakeStage viewChooseMakeStage = UIControllerLIst.GetViewByName<ViewChooseMakeStage>(UIViewName.ViewChooseMakeStage);
                    switch (notification.Type)
                    {
                        //获取下一组数据
                        case UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_LOAD_NEXT_LIST:
                            List<CardEntry> cardList = notification.Body as List<CardEntry>;
                            viewChooseMakeStage = UIControllerLIst.GetViewByName<ViewChooseMakeStage>(UIViewName.ViewChooseMakeStage);
                            viewChooseMakeStage.LoadNewCardList(cardList);
                            break;
                        case UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_ONE_CARD_SUCCESS:

                            CardEntry card = notification.Body as CardEntry;
                            viewChooseMakeStage.ChangeOneCradForBuyed(card);
                            break;
                    }
                    break;
                case UIViewSystemEvent.UI_CARD_DECK_LIST:
                    switch (notification.Type)
                    {
                        //读取牌组
                        case UIViewSystemEvent.UI_CARD_DECK_LIST_LOAD:
                            PlayerItem playerItemLoad = notification.Body as PlayerItem;
                            cardDeckListLoad = UIControllerLIst.GetViewListByName<CardDeckList>(UIViewName.CardDeckList);
                            for (int num = 0; num < cardDeckListLoad.Count; num++)
                            {
                                if (cardDeckListLoad[num].playerItem.playerCode == playerItemLoad.playerCode)
                                {
                                    cardDeckListLoad[num].playerItem = playerItemLoad;
                                    cardDeckListLoad[num].LoadPlayerInfo();
                                    foreach (CardHeadView cardHeadView in cardDeckListLoad[num].cardHeadViews)
                                    {
                                        cardHeadView.card.cardPosition = cardHeadView.transform.position;
                                        cardHeadView.OnPointerEnter = () =>
                                        {
                                            SendNotification(
                                                UIViewSystemEvent.UI_VIEW_CURRENT,
                                                cardHeadView.card,
                                                StringUtil.GetNTByNotificationTypeAndUIViewName(
                                                    UIViewSystemEvent.UI_VIEW_CURRENT_OPEN_ONE_VIEW,
                                                    UIViewConfig.getNameStrByUIViewName(UIViewName.OneCardAllInfo)
                                                    )
                                                );
                                        };
                                        cardHeadView.OnPointerExit = () =>
                                        {
                                            SendNotification(UIViewSystemEvent.UI_VIEW_CURRENT, UIViewConfig.getNameStrByUIViewName(UIViewName.OneCardAllInfo), UIViewSystemEvent.UI_VIEW_CURRENT_CLOSE_ONE_VIEW);
                                        };
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case UIViewSystemEvent.UI_USER_OPERAT:
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_USER_OPERAT_CHOOSE_EFFECT:
                            List<CardEntry> cardEntries = notification.Body as List<CardEntry>;
                            UIControllerLIst.ShowView(UIViewName.ChooseStage);
                            viewChooseStage = UIControllerLIst.GetViewByName<ViewChooseStage>(UIViewName.ChooseStage);
                            //载入卡牌列表
                            viewChooseStage.LoadCardEntryList(cardEntries);
                            //载入完成后绑定事件
                            foreach (CardIntactView cardIntactView in viewChooseStage.cardIntactViews)
                            {
                                //如果是自己的命令则绑定上点击事件
                                if (playerCode == playerCodeNotification)
                                {
                                    cardIntactView.OnClick = () =>
                                    {
                                        SendNotification(OperateSystemEvent.OPERATE_SYS, cardIntactView.card, OperateSystemEvent.OPERATE_SYS_CHOOSE_ONE_EFFECT);
                                    };
                                }
                                else
                                {
                                    //设置为空
                                    cardIntactView.OnClick = () =>
                                    {

                                    };
                                }

                            }
                            break;
                    }
                    break;
                //费用控制组件
                case UIViewSystemEvent.UI_MANA_INFA_SYS:
                    int changeNum = 0;
                    ManaInfoView manaInfoView = null;
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_MANA_INFA_SYS_INIT:
                            ManaItem manaItem = notification.Body as ManaItem;
                            manaInfoView = UIControllerLIst.GetViewByName<ManaInfoView>(UIViewName.ManaInfoView);
                            manaInfoView.UIManaInfoSysInit(manaItem, myself, playerCodeNotification);
                            break;
                        case UIViewSystemEvent.UI_MANA_INFA_SYS_USABLE_CHANGE:
                            changeNum = Convert.ToInt32(notification.Body);
                            manaInfoView = UIControllerLIst.GetViewByName<ManaInfoView>(UIViewName.ManaInfoView);
                            manaInfoView.ChangeManaUsable(changeNum, myself);
                            break;
                        case UIViewSystemEvent.UI_MANA_INFA_SYS_LIMIT_CHANGE:
                            changeNum = Convert.ToInt32(notification.Body);
                            manaInfoView = UIControllerLIst.GetViewByName<ManaInfoView>(UIViewName.ManaInfoView);
                            manaInfoView.ChangeManaUpperLimit(changeNum, myself);
                            break;
                    }
                    break;
                //科技组件显示
                case UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS:
                    TraitCombinationView traitCombinationView = null;
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_INIT:
                            List<TraitType> traitTypes = notification.Body as List<TraitType>;
                            traitCombinationView = UIControllerLIst.GetViewByName<TraitCombinationView>(UIViewName.TraitCombinationView);
                            traitCombinationView.UITraitCombinationSysInit(traitTypes, myself, playerCodeNotification);
                            break;
                        case UIViewSystemEvent.UI_TRAIT_COMBINATION_SYS_ADD:
                            string traitType = notification.Body.ToString();
                            traitCombinationView = UIControllerLIst.GetViewByName<TraitCombinationView>(UIViewName.TraitCombinationView);
                            traitCombinationView.UITraitCombinationSysAdd(traitType, myself);
                            break;
                    }
                    break;
                //效果展示列控制
                case UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS:
                    EffectDisplayView effectDisplayView = null;
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_EFFECT_DISPLAY_SYS_PUT_ONE_EFFECT:
                            effectDisplayView = UIControllerLIst.GetViewByName<EffectDisplayView>(UIViewName.EffectDisplayView);
                            CardEntry effectCard = notification.Body as CardEntry;
                            callBackDelay = true;
                            effectDisplayView.ShowCradEffectByCardEntry(effectCard, callBack);
                            break;
                    }
                    break;
                //科技组件显示
                case UIViewSystemEvent.UI_PLAYER_SCORE_SHOW_SYS:
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_PLAYER_SCORE_SHOW_SYS_CHANGE:
                            ShipComponentView shipScoreComponentView = UIControllerLIst.GetViewByName<ShipComponentView>(UIViewName.ShipComponentView);
                            shipScoreComponentView.ChangeScoreShow(myself,Convert.ToInt32(notification.Body));
                            break;   
                    }
                    break;
                //结束按钮的显示控制
                case UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS:
                    NextTurnButton nextTurnButton = null;
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_NEXT_TURN_SHOW_SYS_SHOW:
                            if (myself) {
                                nextTurnButton = UIControllerLIst.GetViewByName<NextTurnButton>(UIViewName.NextTurnButton);
                                nextTurnButton.ShowButton();
                            }
                            break;
                    }
                    break;
               
            }
            if (callBackDelay == false)
            {
                callBack();
            }
        }


    }
}
