
using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;


namespace Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit
{
    public class ChooseStageCircuitItem
    {
        public string nowPlayerCode { get; set; }
        public List<string> playerOrder { get; set; }

        public Dictionary<string, List<CardInfo>> playerShipCardMap = new Dictionary<string, List<CardInfo>>();



    }
}
