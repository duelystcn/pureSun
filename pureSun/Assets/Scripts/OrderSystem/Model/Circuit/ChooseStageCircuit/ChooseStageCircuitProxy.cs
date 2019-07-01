

using PureMVC.Patterns.Proxy;

namespace Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit
{
    public class ChooseStageCircuitProxy : Proxy
    {
        public new const string NAME = "ChooseStageCircuitProxy";

        public ChooseStageCircuitItem chooseStageCircuitItem
        {
            get { return (ChooseStageCircuitItem)base.Data; }
        }
        public ChooseStageCircuitProxy() : base(NAME)
        {
            ChooseStageCircuitItem chooseStageCircuitItem = new ChooseStageCircuitItem();
            base.Data = chooseStageCircuitItem;
        }
    }
}
