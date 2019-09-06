
using Assets.Scripts.OrderSystem.Model.Circuit.QuestStageCircuit;
using PureMVC.Patterns.Mediator;
using System;

namespace Assets.Scripts.OrderSystem.View.CircuitView.QuestStageCircuit
{
    public class QuestStageCircuitMediator : Mediator
    {
        public new const string NAME = "CircuitMediator";
        private QuestStageCircuitProxy circuitProxy = null;
        public QuestStageCircuitButton circuitButton
        {
            get { return (QuestStageCircuitButton)base.ViewComponent; }
        }
        public QuestStageCircuitMediator(QuestStageCircuitButton circuitButton) : base(NAME, circuitButton)
        {

        }
        public override void OnRegister()
        {
            base.OnRegister();
            circuitProxy = Facade.RetrieveProxy(QuestStageCircuitProxy.NAME) as QuestStageCircuitProxy;
            if (null == circuitProxy)
                throw new Exception(QuestStageCircuitProxy.NAME + "is null.");
            circuitButton.OnClick += () =>
            {
               
            };
        }
    }
}
