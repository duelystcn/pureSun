
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.Animation;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections.Generic;
using System.Threading;
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
            notificationList.Add(UIViewSystemEvent.UI_START_MAIN);
            notificationList.Add(UIViewSystemEvent.UI_CHOOSE_STAGE);
            notificationList.Add(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE);
            notificationList.Add(UIViewSystemEvent.UI_CARD_DECK_LIST);
            notificationList.Add(UIViewSystemEvent.UI_ONE_CARD_ALL_INFO);
            notificationList.Add(UIViewSystemEvent.UI_USER_OPERAT);
            notificationList.Add(UIViewSystemEvent.UI_ANIMATION_SYS);
            AddCommonNotificationInterests(notificationList);
            return notificationList.ToArray();
        }



        public override void HandleNotification(INotification notification)
        {
            if (notification.Name == UIViewSystemEvent.UI_ANIMATION_SYS && notification.Type == UIViewSystemEvent.UI_ANIMATION_SYS_START)
            {
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

                case UIViewSystemEvent.UI_START_MAIN:
                    switch (notification.Type)
                    {
                        //开始菜单打开
                        case UIViewSystemEvent.UI_START_MAIN_OPEN:

                            UIControllerLIst.ShowView(UIViewName.StartMain);
                            UIControllerLIst.ShowView(UIViewName.CardMoveAnimation);
                            ViewStartMain viewMain = UIControllerLIst.GetViewByName<ViewStartMain>(UIViewName.StartMain);
                            viewMain.StartCompleteGameUnityAction += () =>
                            {
                                SendNotification(UIViewSystemEvent.UI_START_MAIN, null, UIViewSystemEvent.UI_START_MAIN_CLOSE);
                                SendNotification(OrderSystemEvent.START_CIRCUIT, null, OrderSystemEvent.START_CIRCUIT_START);
                            };
                            viewMain.StartTestMapUnityAction += () =>
                            {
                                SendNotification(UIViewSystemEvent.UI_START_MAIN, null, UIViewSystemEvent.UI_START_MAIN_CLOSE);
                                SendNotification(OrderSystemEvent.START_CIRCUIT, null, OrderSystemEvent.START_CIRCUIT_TEST_MAP);
                            };
                            break;
                        //开始菜单关闭
                        case UIViewSystemEvent.UI_START_MAIN_CLOSE:
                            UIControllerLIst.HideView(UIViewName.StartMain);
                            break;
                    }
                    break;
                case UIViewSystemEvent.UI_CHOOSE_STAGE:
                    switch (notification.Type)
                    {
                        //选择阶段窗口打开
                        case UIViewSystemEvent.UI_CHOOSE_STAGE_OPEN:

                            UIControllerLIst.ShowView(UIViewName.ChooseStage);

                            break;
                        case UIViewSystemEvent.UI_CHOOSE_STAGE_CLOSE:

                            UIControllerLIst.HideView(UIViewName.ChooseStage);

                            break;
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
                        //打开界面
                        case UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_OPEN:

                            UIControllerLIst.ShowView(UIViewName.ViewChooseMakeStage);
                            List<List<CardEntry>> cardEntries = notification.Body as List<List<CardEntry>>;
                            viewChooseMakeStage = UIControllerLIst.GetViewByName<ViewChooseMakeStage>(UIViewName.ViewChooseMakeStage);
                            viewChooseMakeStage.LoadStartCardList(cardEntries);
                            foreach (ViewChooseStage oneViewChoose in viewChooseMakeStage.sortViewChosseMap.Values)
                            {
                                foreach (CardIntactView cardIntactView in oneViewChoose.cardIntactViews)
                                {
                                    if (cardIntactView.card.layerSort == VCSLayerSort.Three)
                                    {
                                        cardIntactView.OnClick = () =>
                                        {
                                                //提示第三排为展示，不可购买
                                                cardIntactView.OnClick = () =>
                                            {
                                            };

                                        };
                                    }
                                    else
                                    {
                                        cardIntactView.OnClick = () =>
                                        {
                                            SendNotification(UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE, cardIntactView.card, UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_ONE_CARD);
                                        };
                                    }
                                }
                            }


                            break;
                        //关闭界面
                        case UIViewSystemEvent.UI_CHOOSE_MAKE_STAGE_CLOSE:

                            UIControllerLIst.HideView(UIViewName.ViewChooseMakeStage);


                            break;
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

                        //第一次进入界面是打开
                        case UIViewSystemEvent.UI_CARD_DECK_LIST_OPEN:


                            PlayerItem playerItem = notification.Body as PlayerItem;
                            UIControllerLIst.ShowView(UIViewName.CardDeckList);
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
                                            SendNotification(UIViewSystemEvent.UI_ONE_CARD_ALL_INFO, cardHeadView.card, UIViewSystemEvent.UI_ONE_CARD_ALL_INFO_OPEN);
                                        };
                                        cardHeadView.OnPointerExit = () =>
                                        {
                                            SendNotification(UIViewSystemEvent.UI_ONE_CARD_ALL_INFO, cardHeadView.card, UIViewSystemEvent.UI_ONE_CARD_ALL_INFO_CLOSE);
                                        };
                                    }
                                }
                            }


                            break;
                    }
                    break;
                case UIViewSystemEvent.UI_ONE_CARD_ALL_INFO:
                    switch (notification.Type)
                    {
                        case UIViewSystemEvent.UI_ONE_CARD_ALL_INFO_OPEN:

                            CardEntry cardEntry = notification.Body as CardEntry;
                            UIControllerLIst.ShowView(UIViewName.OneCardAllInfo);
                            OneCardAllInfo oneCardAllInfo = UIControllerLIst.GetViewByName<OneCardAllInfo>(UIViewName.OneCardAllInfo);
                            oneCardAllInfo.LoadCardInfo(cardEntry);



                            break;
                        case UIViewSystemEvent.UI_ONE_CARD_ALL_INFO_CLOSE:
                            UIControllerLIst.HideView(UIViewName.OneCardAllInfo);
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
                        case UIViewSystemEvent.UI_USER_OPERAT_CHOOSE_EFFECT_OVER:
                            UIControllerLIst.HideView(UIViewName.ChooseStage);
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
