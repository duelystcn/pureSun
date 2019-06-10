

using PureMVC.Patterns.Mediator;

namespace Assets.Scripts.OrderSystem.View.SpecialOperateView.ChooseView
{
    class ChooseGridMediator : Mediator
    {
        public new const string NAME = "ChooseGridMediator";

        public ChooseGridView chooseGridView
        {
            get { return (ChooseGridView)base.ViewComponent; }
        }
        public ChooseGridMediator(ChooseGridView chooseGridView) : base(NAME, chooseGridView)
        {

        }

    }
}
