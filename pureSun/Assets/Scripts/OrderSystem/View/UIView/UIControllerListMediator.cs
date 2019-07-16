
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
    public class UIControllerListMediator : Mediator
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
            string[] notifications = new string[3];
            notifications[0] = UIViewSystemEvent.UI_START_MAIN;
            notifications[1] = UIViewSystemEvent.UI_CHOOSE_STAGE;
            notifications[2] = UIViewSystemEvent.UI_CARD_DECK_LIST;
            return notifications;
        }
        public override void HandleNotification(INotification notification)
        {
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
                            viewMain.unityAction += () =>
                            {
                                SendNotification(OrderSystemEvent.START_CIRCUIT, null, OrderSystemEvent.START_CIRCUIT_START);
                            };
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
                                        position.x = -570;
                                       
                                    }
                                    else {
                                        position.x = 400;
      
                                    }

                                    cardDeckLists[num].transform.localPosition = position;
                                    cardDeckLists[num].LoadPlayerInfo();
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
                                }
                            }
                            break;


                    }
                    break;


            }

        }
    }
}
