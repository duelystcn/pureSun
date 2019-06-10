

using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Circuit
{
    public class CircuitItem
    {
        public int turnNum { get;  set; }
        public string nowPlayerCode { get;  set; }
        public List<string> playerOrder { get; set; }


    }
}
