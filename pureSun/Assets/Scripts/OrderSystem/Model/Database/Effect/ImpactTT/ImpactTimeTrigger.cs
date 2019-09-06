
namespace Assets.Scripts.OrderSystem.Model.Database.Effect.ImpactTT
{
    public class ImpactTimeTrigger
    {
        //名称
        public string name { get; set; }
        //描述
        public string description { get; set; }

        public string impactTimeTriggertMonitor { get; set; }

        public string[] impactTimeTriggertClaims { get; set; }

        public string[] impactTimeTriggertClaimsContents { get; set; }
    }
}
