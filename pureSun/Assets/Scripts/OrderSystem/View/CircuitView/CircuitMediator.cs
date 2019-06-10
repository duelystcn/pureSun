using Assets.Scripts.OrderSystem.Model.Circuit;
using PureMVC.Patterns.Mediator;
using System;
using UnityEngine;

namespace Assets.Scripts.OrderSystem.View.CircuitView
{
    public class CircuitMediator : Mediator
    {
        public new const string NAME = "CircuitMediator";
        private CircuitProxy circuitProxy = null;
        public CircuitButton circuitButton
        {
            get { return (CircuitButton)base.ViewComponent; }
        }
        public CircuitMediator(CircuitButton circuitButton) : base(NAME, circuitButton)
        {

        }
        public override void OnRegister()
        {
            base.OnRegister();
            circuitProxy = Facade.RetrieveProxy(CircuitProxy.NAME) as CircuitProxy;
            if (null == circuitProxy)
                throw new Exception(CircuitProxy.NAME + "is null.");
            circuitButton.OnClick += () =>
            {
                circuitProxy.IntoNextTurn();
            };
        }
    }
}
