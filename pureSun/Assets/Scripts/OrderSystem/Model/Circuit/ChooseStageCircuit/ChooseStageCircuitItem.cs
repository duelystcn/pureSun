
using Assets.Scripts.OrderSystem.Model.Database.Card;
using System.Collections.Generic;


namespace Assets.Scripts.OrderSystem.Model.Circuit.ChooseStageCircuit
{
    public class ChooseStageCircuitItem
    {
        public enum ChooseStage { ShipStage,CardStage  };

        public ChooseStage WhichStage = ChooseStage.ShipStage;

        public string nowPlayerCode { get; set; }
        public List<string> playerOrder { get; set; }

        public Dictionary<string, List<CardEntry>> playerShipCardMap = new Dictionary<string, List<CardEntry>>();

        public int chooseIndex = 0;



    }
}
