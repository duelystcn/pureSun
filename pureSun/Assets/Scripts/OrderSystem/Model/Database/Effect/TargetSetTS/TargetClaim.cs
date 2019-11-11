
using System.Collections.Generic;

namespace Assets.Scripts.OrderSystem.Model.Database.Effect.TargetSetTS
{
    public class TargetClaim
    {
        public string claim { get; set; }

        public string content { get; set; }

        public List<string> result = new List<string>();
    }
}
