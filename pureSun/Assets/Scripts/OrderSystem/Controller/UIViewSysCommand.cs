using Assets.Scripts.OrderSystem.Event;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Assets.Scripts.OrderSystem.Controller
{
    internal class UIViewSysCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Type)
            {
                case UIViewSystemEvent.UI_CHOOSE_STAGE_START:
                    break;
            }
        }

    }
}
