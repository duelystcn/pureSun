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
                case TimeTriggerEvent.TIME_TRIGGER_SYS_DRAW_A_CARD:
                    break;
            }
        }

    }
}
