
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.Model.Player;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections.Generic;
using UnityEngine;

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
            AddCommonNotificationInterests(notificationList);
            return notificationList.ToArray();
        }
        public override void HandleNotification(INotification notification)
        {
            //处理公共请求
            HandleNotificationCommon(notification);
            ViewChooseStage viewChooseStage = null;
            switch (notification.Name)
            {
                case UIViewSystemEvent.UI_START_MAIN:
                    switch (notification.Type)
                    {
                        //开始菜单打开
                        case UIViewSystemEvent.UI_START_MAIN_OPEN:
                            UIControllerLIst.ShowView(UIViewName.StartMain);
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
                        //开始菜单打开
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

                        case UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_INFO:
                            viewChooseStage = UIControllerLIst.GetViewByName<ViewChooseStage>(UIViewName.ChooseStage);
                            List<CardInfo> shipCardInfoList = (List<CardInfo>)notification.Body;
                            //载入卡牌列表
                            viewChooseStage.LoadCardInfoList(shipCardInfoList);
                            //载入完成后绑定事件
                            foreach (CardIntactView cardIntactView in viewChooseStage.cardIntactViews)
                            {
                                cardIntactView.OnClick = () =>
                                {
                                    SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, cardIntactView.card, UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD);
                                };
                            }
                            break;
                        case UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD_ENTRY:
                            viewChooseStage = UIControllerLIst.GetViewByName<ViewChooseStage>(UIViewName.ChooseStage);
                            List<CardEntry> shipCardEntryList = (List<CardEntry>)notification.Body;
                            //载入卡牌列表
                            viewChooseStage.LoadCardEntryList(shipCardEntryList);
                            //载入完成后绑定事件
                            foreach (CardIntactView cardIntactView in viewChooseStage.cardIntactViews)
                            {
                                cardIntactView.OnClick = () =>
                                {
                                    SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, cardIntactView.card, UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD);
                                };
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
                            foreach (ViewChooseStage oneViewChoose in viewChooseMakeStage.sortViewChosseMap.Values) {
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
                                    else {
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
                            List<CardEntry> cardList= notification.Body as List<CardEntry>;
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
                            for (int num = 0; num < cardDeckLists.Count; num++) {
                                if (cardDeckLists[num].playerItem == null)
                                {
                                    cardDeckLists[num].playerItem = playerItem;
                                    //设置位置
                                    //0在左，1在右
                                    Vector3 position = new Vector3();
                                    if (num == 0)
                                    {
                                        position.x = -600;
                                       
                                    }
                                    else {
                                        position.x = 430;
      
                                    }
                                    cardDeckLists[num].transform.localPosition = position;
                                }
                            }
                            break;
                        //读取牌组
                        case UIViewSystemEvent.UI_CARD_DECK_LIST_LOAD:
                            PlayerItem playerItemLoad = notification.Body as PlayerItem;
                            List<CardDeckList> cardDeckListLoad = UIControllerLIst.GetViewListByName<CardDeckList>(UIViewName.CardDeckList);
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
                            UtilityLog.Log("user choose effect");
                            break;
                    }
                    break;

            }

        }
    }
}
