
using Assets.Scripts.OrderSystem.Common.UnityExpand;
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.Model.Database.Card;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.ComponentView;
using OrderSystem;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections.Generic;

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
            string[] notifications = new string[2];
            notifications[0] = UIViewSystemEvent.UI_START_MAIN;
            notifications[1] = UIViewSystemEvent.UI_CHOOSE_STAGE;
            return notifications;
        }
        public override void HandleNotification(INotification notification)
        {
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

                        case UIViewSystemEvent.UI_CHOOSE_STAGE_LOAD_CARD:
                            ViewChooseStage viewChooseStage = UIControllerLIst.GetViewByName<ViewChooseStage>(UIViewName.ChooseStage);
                            List<CardInfo> shipCardList = (List<CardInfo>)notification.Body;
                            //载入卡牌列表
                            viewChooseStage.LoadCardList(shipCardList);
                            //载入完成后绑定事件
                            foreach (CardIntactView cardIntactView in viewChooseStage.cardIntactViews)
                            {
                                cardIntactView.OnClick += () =>
                                {
                                    SendNotification(UIViewSystemEvent.UI_CHOOSE_STAGE, cardIntactView.card, UIViewSystemEvent.UI_CHOOSE_STAGE_ONE_CARD);
                                };
                            }
                            break;
                    }
                    break;



            }

        }
    }
}
