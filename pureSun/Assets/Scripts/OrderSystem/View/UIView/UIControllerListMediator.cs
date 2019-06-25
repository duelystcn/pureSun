
using Assets.Scripts.OrderSystem.Event;
using Assets.Scripts.OrderSystem.View.UIView.UISonView.BaseView;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;

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
            string[] notifications = new string[1];
            notifications[0] = UIViewSystemEvent.UI_START_MAIN;
            return notifications;
        }
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case UIViewSystemEvent.UI_START_MAIN:
                    ViewStartMain viewMain = UIControllerLIst.GetViewByName<ViewStartMain>(UIViewName.StartMain);

                    break;


            }

        }
    }
}
